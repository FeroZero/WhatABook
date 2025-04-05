using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Compras
{
	[Key]
	public int CompraId { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio.")]
	public DateTime Fecha { get; set; } = DateTime.Now;

	[ForeignKey("CompraId")]
	public ICollection<ComprasDetalle> CompraDetalle { get; set; } = new List<ComprasDetalle>();
}

