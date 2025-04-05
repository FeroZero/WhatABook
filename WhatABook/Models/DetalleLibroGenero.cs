using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models
{
	public class DetalleLibroGenero
	{
		[Key]
		public int DetalleLibroGeneroId { get; set; }

		public int LibroId { get; set; }

		public int GeneroId { get; set; }

		[ForeignKey("LibroId")]
		public Libros? Libro { get; set; }

		[ForeignKey("GeneroId")]
		public Generos? Genero { get; set; }
	}
}
