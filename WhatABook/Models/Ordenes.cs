using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models
{
	public class Ordenes
	{
		[Key]
		public int OrdenId { get; set; }

		[ForeignKey("ApplicationUser")]
		public int UsuarioId { get; set; }

		public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

		public ICollection<OrdenesDetalle> ordenes { get; set; } = new List<OrdenesDetalle>();
	}
}
