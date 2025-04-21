// ... usings se mantienen igual ...

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WhatABook.Data;
using WhatABook.Enum;
using WhatABook.Models;

namespace WhatABook.Services;

public class OrdenesService(IDbContextFactory<ApplicationDbContext> DbFactory, IHttpContextAccessor httpContextAccessor)
{
	private string ObtenerUsuarioId()
	{
		var httpContext = httpContextAccessor.HttpContext;
		if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
		{
			throw new InvalidOperationException("El usuario no está autenticado.");
		}

		var idClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
		if (idClaim == null)
		{
			throw new InvalidOperationException("No se encontró el claim del identificador del usuario.");
		}

		return idClaim.Value; // No uses int.Parse
	}


	public async Task<List<OrdenesDetalle>> ObtenerOrdenes()
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.OrdenesDetalle
			.Include(d => d.Libros)
			.Where(d => d.Ordenes.UsuarioId == usuarioId && d.Ordenes.Estado == EstadoOrden.Pendiente)
			.ToListAsync();
	}

	public async Task<bool> AgregarOrdenes(int libroId, int cantidad)
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		await using var transaction = await contexto.Database.BeginTransactionAsync();

		try
		{
			var libro = await contexto.Libros.FindAsync(libroId);
			if (libro == null || libro.Cantidad < cantidad)
				return false;

			var orden = await contexto.Ordenes
				.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

			if (orden == null)
			{
				orden = new Ordenes
				{
					UsuarioId = usuarioId,
					Estado = EstadoOrden.Pendiente,
					Fecha = DateTime.Now,
					MetodoDePago = null,
					MontoTotal = 0
				};
				contexto.Ordenes.Add(orden);
				await contexto.SaveChangesAsync();
			}

			var detalleExistente = await contexto.OrdenesDetalle
				.FirstOrDefaultAsync(d => d.OrdenId == orden.OrdenId && d.LibroId == libroId);

			if (detalleExistente == null)
			{
				var nuevoDetalle = new OrdenesDetalle
				{
					OrdenId = orden.OrdenId,
					LibroId = libroId,
					Cantidad = cantidad,
					Monto = libro.Precio * cantidad
				};
				contexto.OrdenesDetalle.Add(nuevoDetalle);
			}
			else
			{
				if (libro.Cantidad >= detalleExistente.Cantidad + cantidad)
				{
					detalleExistente.Cantidad += cantidad;
					detalleExistente.Monto = detalleExistente.Cantidad * libro.Precio;
				}
				else
				{
					return false;
				}
			}

			orden.MontoTotal = orden.ordenes.Sum(d => d.Monto);

			await contexto.SaveChangesAsync();
			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<bool> ActualizarCantidadLibro(int detalleId, int nuevaCantidad)
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		await using var transaction = await contexto.Database.BeginTransactionAsync();

		try
		{
			var detalle = await contexto.OrdenesDetalle
				.Include(d => d.Libros)
				.FirstOrDefaultAsync(d => d.DetalleId == detalleId && d.Ordenes.UsuarioId == usuarioId);

			if (detalle == null || detalle.Libros == null || nuevaCantidad < 1 || nuevaCantidad > detalle.Libros.Cantidad)
				return false;

			detalle.Cantidad = nuevaCantidad;
			detalle.Monto = detalle.Cantidad * detalle.Libros.Precio;

			await contexto.SaveChangesAsync();
			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<bool> EliminarLibroDelCarrito(int detalleId)
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		await using var transaction = await contexto.Database.BeginTransactionAsync();

		try
		{
			var detalle = await contexto.OrdenesDetalle
				.Include(d => d.Ordenes)
				.FirstOrDefaultAsync(d => d.DetalleId == detalleId && d.Ordenes.UsuarioId == usuarioId);

			if (detalle == null)
				return false;

			contexto.OrdenesDetalle.Remove(detalle);
			detalle.Ordenes.MontoTotal = detalle.Ordenes.ordenes.Sum(d => d.Monto);

			await contexto.SaveChangesAsync();
			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<bool> VaciarCarrito()
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		await using var transaction = await contexto.Database.BeginTransactionAsync();

		try
		{
			var orden = await contexto.Ordenes
				.Include(o => o.ordenes)
				.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

			if (orden == null)
				return false;

			contexto.OrdenesDetalle.RemoveRange(orden.ordenes);
			orden.MontoTotal = 0;

			await contexto.SaveChangesAsync();
			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<bool> ComprarCarrito()
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		await using var transaction = await contexto.Database.BeginTransactionAsync();

		try
		{
			var orden = await contexto.Ordenes
				.Include(o => o.ordenes)
				.ThenInclude(d => d.Libros)
				.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

			if (orden == null)
				return false;

			foreach (var detalle in orden.ordenes)
			{
				if (detalle.Libros.Cantidad < detalle.Cantidad)
				{
					await transaction.RollbackAsync();
					return false;
				}
			}

			foreach (var detalle in orden.ordenes)
			{
				detalle.Libros.Cantidad -= detalle.Cantidad;
				contexto.Libros.Update(detalle.Libros);
			}

			orden.Estado = EstadoOrden.Completada;

			await contexto.SaveChangesAsync();
			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<int> ObtenerCantidadLibrosEnCarrito()
	{
		string usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		var orden = await contexto.Ordenes
			.Include(o => o.ordenes)
			.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

		return orden?.ordenes.Sum(d => d.Cantidad) ?? 0;
	}

	//public async Task FinalizarOrden(int metodoDePagoId)
	//{
	//	string usuarioId = ObtenerUsuarioId();

	//	await using var contexto = await DbFactory.CreateDbContextAsync();
	//	var orden = await contexto.Ordenes
	//		.Include(o => o.ordenes)
	//		.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

	//	if (orden != null)
	//	{
	//		orden.Estado = EstadoOrden.Completada;
	//		orden.Fecha = DateTime.Now;
	//		orden.MetodoDePago = metodoDePago;
	//		orden.MontoTotal = orden.ordenes.Sum(d => d.Monto * d.Cantidad);
	//		await contexto.SaveChangesAsync();
	//	}
	//}

	public async Task FinalizarOrden(int? metodoPagoId)
	{
		var usuarioId = ObtenerUsuarioId();

		await using var contexto = await DbFactory.CreateDbContextAsync();
		var orden = await contexto.Ordenes
			.Include(o => o.ordenes)
			.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId && o.Estado == EstadoOrden.Pendiente);

		if (orden == null)
			return;

		orden.MetodoDePagoId = metodoPagoId;
		orden.Estado = EstadoOrden.Completada;
		orden.Fecha= DateTime.Now;
		orden.MontoTotal = orden.ordenes.Sum(d => d.Monto);

		await contexto.SaveChangesAsync();
	}
}
