using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class OrdenesDetalle
{
	[Key]
	public int DetalleId { get; set; }
	public int Cantidad { get; set; }
	public double Monto { get; set; }

	[ForeignKey("Ordenes")]
	public int OrdenId{ get; set; }
	public Ordenes ordenes { get; set; }

	[ForeignKey("Libros")]
	public int LibroId { get; set; }
	public Libros libros { get; set; }
}
