using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models
{
	public class GenerosDetalle
	{
		[Key]
		public int DetalleId { get; set; }

		[ForeignKey("Libros")]
		public int LibroId { get; set; }
		public Libros Libros { get; set; } = new Libros();
		
		[ForeignKey("Generos")]
		public int GeneroId { get; set; }
		public Generos Generos { get; set; } = new Generos();

	}
}
