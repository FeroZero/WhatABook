using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services;

public class LibrosService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<bool> Guardar(Libros libro)
	{
		if (!await Existe(libro.LibroId))
			return await Insertar(libro);
		else
			return await Modificar(libro);
	}

	private async Task<bool> Existe(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.AnyAsync(p => p.LibroId == id);
	}

	public async Task<bool> Modificar(Libros libro)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		// Cargar el libro existente desde la base de datos
		var libroExistente = await contexto.Libros
			.Include(l => l.ListaGeneros)
			.ThenInclude(dlg => dlg.Genero)
			.FirstOrDefaultAsync(l => l.LibroId == libro.LibroId);

		if (libroExistente == null)
			return false; // Si no se encuentra, no hay nada que modificar

		// Manejar géneros removidos
		foreach (var detalleExistente in libroExistente.ListaGeneros.ToList())
		{
			if (!libro.ListaGeneros.Any(d => d.GeneroId == detalleExistente.GeneroId))
			{
				// Marcar género removido como eliminado
				contexto.Entry(detalleExistente).State = EntityState.Deleted;
			}
		}

		// Actualizar los géneros restantes
		foreach (var nuevoDetalle in libro.ListaGeneros)
		{
			var generoExistente = await contexto.Generos
				.FirstOrDefaultAsync(g => g.GeneroId == nuevoDetalle.GeneroId);

			if (generoExistente != null)
			{
				nuevoDetalle.Genero = generoExistente; // Reutilizar instancia existente
			}
			else
			{
				contexto.Entry(nuevoDetalle).State = EntityState.Added; // Nuevo género
			}
		}

		// Actualizar campos del libro principal
		libroExistente.Titulo = libro.Titulo;
		libroExistente.Autores = libro.Autores;
		libroExistente.Cantidad = libro.Cantidad;
		libroExistente.Precio = libro.Precio;
		libroExistente.Descripcion = libro.Descripcion;
		libroExistente.FechaPublicacion = libro.FechaPublicacion;
		libroExistente.ImagenUrl = libro.ImagenUrl; // Ejemplo de actualización
		libroExistente.ListaGeneros = libro.ListaGeneros;

		// Guardar los cambios en la base de datos
		return await contexto.SaveChangesAsync() > 0;
	}


	private async Task<bool> Insertar(Libros libro)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		foreach (var detalle in libro.ListaGeneros)
		{
			if (detalle.Genero != null)
			{
				contexto.Entry(detalle.Genero).State = EntityState.Unchanged;
			}
		}

		contexto.Libros.Add(libro);

		return await contexto.SaveChangesAsync() > 0;
	}

	public async Task<bool> Eliminar(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.Where(p => p.LibroId == id)
			.ExecuteDeleteAsync() > 0;
	}

	public async Task<Libros?> Buscar(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.Include(l => l.ListaGeneros)
			.ThenInclude(g => g.Genero)
			.AsNoTracking() // No rastrear las entidades cargadas
			.FirstOrDefaultAsync(l => l.LibroId == id);
	}
	public async Task<List<Libros>> Listar()
	{
		await using var context = await DbFactory.CreateDbContextAsync();
		return await context.Libros
			.AsNoTracking()
			.Include(l => l.ListaGeneros)
			.ThenInclude(g => g.Genero)
			.ToListAsync();
	}
	public async Task<bool> ExisteLibro(string titulo, int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.AnyAsync(t => t.Titulo.ToLower().Equals(titulo.ToLower()) && t.LibroId != id);
	}
}
