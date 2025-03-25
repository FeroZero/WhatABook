using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models
{
	public class OrdenesDetalle
	{
		[Key]
		public int DetalleId { get; set; }

		public int UsuarioId { get; set; }

		public int LibroId { get; set; }

		
	}
}
