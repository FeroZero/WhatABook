using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatABook.Models;

namespace WhatABook.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<Libros> Libros { get; set; }
	public DbSet<Generos> Generos { get; set; }
	public DbSet<Ordenes> Ordenes { get; set; }
	public DbSet<Compras> Compras { get; set; }
	public DbSet<MetodosDePagos> MetodosDePagos { get; set; }
	public DbSet<OrdenesDetalle> OrdenesDetalle { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		ConfigureGeneralModel(modelBuilder);
	}

	public void ConfigureGeneralModel(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MetodosDePagos>().HasData(
			 new MetodosDePagos { MetodoPagoId = 1, Descripcion = "Efectivo" },
			 new MetodosDePagos { MetodoPagoId = 2, Descripcion = "Tarjeta" }
		);

			modelBuilder.Entity<Generos>().HasData(
				new Generos { GeneroId = 1, TipoGeneros = "Acción" },
				new Generos { GeneroId = 2, TipoGeneros = "Aventura" },
				new Generos { GeneroId = 3, TipoGeneros = "Ciencia Ficción" },
				new Generos { GeneroId = 4, TipoGeneros = "Fantasía" },
				new Generos { GeneroId = 5, TipoGeneros = "Romance" },
				new Generos { GeneroId = 6, TipoGeneros = "Horror" },
				new Generos { GeneroId = 7, TipoGeneros = "Literatura juvenil" },
				new Generos { GeneroId = 8, TipoGeneros = "Poesía" }
			);
	}
}
