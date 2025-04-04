﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Compras
{
	[Key]
	public int CompraId { get; set; }

	[StringLength(70, ErrorMessage = "Límite Excedido.")]
	[Required(ErrorMessage = "Campo Obligatorio.")]
	public string LibroNombre { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio.")]
	[Range(1, 100, ErrorMessage = "Porcentaje Excedido.")]
	public double PorcentajeGanancia { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio.")]
	[Range(0.01, 1000000, ErrorMessage = "Costo Excedido.")]
	public double Costo { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio.")]
	[Range(1, 200, ErrorMessage = "Cantidad Excedida.")]
	public int Cantidad { get; set; }

	[Required]
	public double PrecioVenta { get; set; }

	[Required]
	public double PrecioCompra { get; set; }

	[ForeignKey("Libro")]
	public int LibroId { get; set; }

	public Libros Libro { get; set; }
}

