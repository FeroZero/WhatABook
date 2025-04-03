using Microsoft.EntityFrameworkCore;
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
		contexto.Update(libro);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Insertar(Libros libro)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
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
			.FirstOrDefaultAsync(p => p.LibroId == id);
	}
	public async Task<List<Libros>> Listar(Expression<Func<Libros, bool>> criterio)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.Include(g => g.Generos)
			.Where(criterio)
			.ToListAsync();
	}
	public async Task<bool> ExisteLibro(string titulo, int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Libros
			.AnyAsync(t => t.Titulo.ToLower().Equals(titulo.ToLower()) && t.LibroId != id);
	}

	//Posible funcion para agregar imagen.
}
