using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatABook.Models;

namespace WhatABook.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Libros> Libros { get; set; }
	public DbSet<Generos> Generos { get; set; }
	public DbSet<Ordenes> Ordenes { get; set; }
	public DbSet<Compras> Compras { get; set; }
	public DbSet<OrdenesDetalle> OrdenesDetalle { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Libros>()
		.HasOne(l => l.Compras)
		.WithOne(c => c.Libros)
		.HasForeignKey<Compras>(c => c.LibroId);

		modelBuilder.Entity<Generos>().HasData(
			new Generos { GeneroId = 1, TipoGeneros = "Accion"},
			new Generos { GeneroId = 2, TipoGeneros = "Drama"},
			new Generos { GeneroId = 3, TipoGeneros = "Misterio"},
			new Generos { GeneroId = 4, TipoGeneros = "Fantasia"},
			new Generos { GeneroId = 5, TipoGeneros = "Romantica"}
			);

		modelBuilder.Entity<Libros>().HasData(
			new Libros { LibroId = 1, Autores = "mango", Descripcion = "hola", Titulo = "Muerto", ImagenUrl = "Cosa", GeneroId = 1, FechaPublicacion = new DateTime(2004, 3, 2)}
			);

		modelBuilder.Entity<Compras>().HasData(
			new Compras { LibroId = 1, LibroNombre = "Julio Ceasar", Cantidad = 40, Costo = 400, PorcentajeGanancia = 33}
			);
	}
}
