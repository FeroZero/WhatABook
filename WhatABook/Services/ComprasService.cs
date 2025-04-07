using WhatABook.Models;
using Microsoft.EntityFrameworkCore;
using WhatABook.Data;
using System.Linq.Expressions;

namespace WhatABook.Services;

public class ComprasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<List<Compras>> Listar()
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Compras
			.Include(d => d.CompraDetalle)
			.ThenInclude(l => l.Libro)
			.ToListAsync();
	}

	public async Task<Compras?> Buscar(int id)
	{
		await using var contexo = await DbFactory.CreateDbContextAsync();
		return await contexo.Compras
			.Include(l => l.CompraDetalle)
			.ThenInclude(l => l.Libro)
			.FirstOrDefaultAsync(p => p.CompraId == id);
	}

	public async Task<bool> Guardar(Compras compra)
	{
		if (!await Existe(compra.CompraId))
			return await Insertar(compra);
		else
			return await Modificar(compra);
	}

	private async Task<bool> Existe(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Compras
			.AnyAsync(p => p.CompraId == id);
	}
	private async Task<bool> Modificar(Compras compra)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		contexto.Update(compra);
		return await contexto.SaveChangesAsync() > 0;
	}
	public async Task<bool> Insertar(Compras compra)
	{
		await using var context = await DbFactory.CreateDbContextAsync();

		foreach (var detalle in compra.CompraDetalle)
		{
			if (detalle.Libro != null)
			{
				// Recupera el libro de la base de datos para evitar conflictos de rastreo
				var libroExistente = await context.Libros
					.Include(l => l.ListaGeneros) // Incluye los géneros relacionados
					.ThenInclude(g => g.Genero)
					.FirstOrDefaultAsync(l => l.LibroId == detalle.Libro.LibroId);

				if (libroExistente != null)
				{
					// Asocia el libro existente al detalle
					detalle.Libro = libroExistente;

					// Asegúrate de que los géneros relacionados no sean rastreados como nuevas instancias
					foreach (var generoDetalle in libroExistente.ListaGeneros)
					{
						context.Entry(generoDetalle.Genero).State = EntityState.Unchanged;
					}
				}
			}
		}

		// Agrega la compra
		context.Compras.Add(compra);

		// Guarda los cambios
		return await context.SaveChangesAsync() > 0;
	}

}