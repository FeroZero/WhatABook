using WhatABook.Models;
using Microsoft.EntityFrameworkCore;
using WhatABook.Data;
using System.Linq.Expressions;

namespace WhatABook.Services;

public class ComprasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{

	public async Task<List<Compras>> Listar(Expression<Func<Compras, bool>> criterio)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Compras
			.Include(g => g.Libros)
			.Where(criterio)
			.ToListAsync();
	}

	public async Task<bool> Guardar(Compras compra)
	{
		if (!await Existe(compra.LibroId))
			return await Insertar(compra);
		else
			return await Modificar(compra);
	}

	private async Task<bool> Existe(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Compras
			.AnyAsync(p => p.LibroId == id);
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
		context.Compras.Add(compra);
		return await context.SaveChangesAsync() > 0;
	}
}