using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services;

public class GenerosService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<List<Generos>> Listar()
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.ToListAsync();
	}

	public async Task<Generos?> Buscar(int id)
	{
		await using var contexo = await DbFactory.CreateDbContextAsync();
		return await contexo.Generos
			.FirstOrDefaultAsync(p => p.GeneroId == id);
	}
}
