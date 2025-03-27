using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatABook.Models;

namespace WhatABook.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
		public DbSet<Libros> Libros { get; set; }
		public DbSet<Generos> Generos { get; set; }
		public DbSet<Ordenes> Ordenes { get; set; }
		public DbSet<Compras> Compras { get; set; }
		public DbSet<OrdenesDetalle> OrdenesDetalle { get; set; }
		public DbSet<GenerosDetalle> GenerosDetalle { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Compras>()
				.HasOne(c => c.Libros)  // Una compra tiene un libro
				.WithOne(l => l.Compras) // Un libro tiene una compra
				.HasForeignKey<Compras>(c => c.LibroId); // Clave foránea en Compras
		}
	}
}
