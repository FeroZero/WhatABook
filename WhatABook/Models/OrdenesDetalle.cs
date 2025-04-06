using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class OrdenesDetalle
{
	[Key]
	public int DetalleId { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio")]
	public int Cantidad { get; set; }

	[ForeignKey("Libros")]
	public int LibroId { get; set; }

	[Required(ErrorMessage = "Monto Obligatorio")]
	[Range(0.01, 1000000, ErrorMessage = "El monto debe ser mayor a 0.01 y menor a 1,000,000")]
	public double Monto { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio")]
	public Libros? Libros { get; set; }

	[ForeignKey("Ordenes")]
	public int OrdenId { get; set; }
	public Ordenes? Ordenes { get; set; }
}