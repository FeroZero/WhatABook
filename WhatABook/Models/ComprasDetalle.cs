using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models
{
	public class ComprasDetalle
	{
		[Key]
		public int CompraDetalleId { get; set; } 

		public int CompraId { get; set; }

		public int LibroId { get; set; }

		[ForeignKey("LibroId")]
		public Libros? Libro { get; set; }

		[Required(ErrorMessage = "Campo Obligatorio.")]
		[Range(1, 100, ErrorMessage = "Porcentaje Excedido.")]
		public double PorcentajeGanancia { get; set; }

		[Required(ErrorMessage = "Campo Obligatorio.")]
		[Range(1, 200, ErrorMessage = "Cantidad Excedida.")]
		public int Cantidad { get; set; }

		[Required(ErrorMessage = "Campo Obligatorio")]
		[Range(0.01, 1000000, ErrorMessage = "Precio de venta Excedido.")]
		public double PrecioVenta { get; set; }

		[Required(ErrorMessage = "Campo Obligatorio")]
		[Range(0.01, 1000000, ErrorMessage = "Precio de compra Excedido.")]
		public double PrecioCompra { get; set; }
	}
}
