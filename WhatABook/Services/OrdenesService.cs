using Microsoft.EntityFrameworkCore;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services
{
	public class OrdenesService(IDbContextFactory<ApplicationDbContext> DbFactory)
	{
		public async Task<List<Ordenes>> Listar()
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ordenes
				.Include(d => d.ordenes)
				.ToListAsync();
		}

		public async Task<Ordenes?> Buscar(int id)
		{
			await using var contexo = await DbFactory.CreateDbContextAsync();
			return await contexo.Ordenes
				.Include(l => l.ordenes)
				.FirstOrDefaultAsync(p => p.OrdenId == id);
		}

		public async Task<bool> Guardar(Ordenes orden)
		{
			if (!await Existe(orden.OrdenId))
				return await Insertar(orden);
			else
				return await Modificar(orden);
		}

		private async Task<bool> Existe(int id)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ordenes
				.AnyAsync(p => p.OrdenId == id);
		}
		private async Task<bool> Modificar(Ordenes orden)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			contexto.Update(orden);
			return await contexto.SaveChangesAsync() > 0;
		}
		public async Task<bool> Insertar(Ordenes orden)
		{
			await using var context = await DbFactory.CreateDbContextAsync();
			context.Ordenes.Add(orden);
			return await context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Eliminar(int id)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ordenes
				.Where(p => p.OrdenId == id)
				.ExecuteDeleteAsync() > 0;
		}
	}
}
