using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models
{
	public class Ordenes
	{
		[Key]
		public int OrdenId { get; set; }

		[ForeignKey("Usuario")]
		public int UsuarioId { get; set; }

		public ICollection<OrdenesDetalle> ordenes { get; set; } = new List<OrdenesDetalle>();
	}
}
