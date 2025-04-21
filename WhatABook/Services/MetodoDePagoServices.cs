using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services
{
	public class MetodoDePagoServices(IDbContextFactory<ApplicationDbContext> DbFactory)
	{
		public async Task<List<MetodosDePagos>> Listar()
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.MetodosDePagos
				.ToListAsync();
		}
	}
}
