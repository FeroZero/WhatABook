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

	private async Task<bool> Modificar(Libros libro)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		// evitar que se inserten géneros existentes
		foreach (var detalle in libro.ListaGeneros)
		{
			if (detalle.Genero != null)
				contexto.Entry(detalle.Genero).State = EntityState.Unchanged;
		}

		contexto.Update(libro);
		return await contexto.SaveChangesAsync() > 0;
	}


	private async Task<bool> Insertar(Libros libro)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		// evitar que se inserten géneros existentes
		foreach (var detalle in libro.ListaGeneros)
		{
			if (detalle.Genero != null)
				contexto.Entry(detalle.Genero).State = EntityState.Unchanged;
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
		await using var contexo = await DbFactory.CreateDbContextAsync();
		return await contexo.Libros
			.Include(g => g.ListaGeneros)
			.ThenInclude(g => g.Genero)
			.FirstOrDefaultAsync(l => l.LibroId == id);
	}
	public async Task<List<Libros>> Listar()
	{
		await using var context = await DbFactory.CreateDbContextAsync();
		return await context.Libros
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
