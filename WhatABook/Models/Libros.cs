using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Libros
{
	[Key]
	public int LibroId { get; set; }

	[RegularExpression(@"[A-Za-z0-9\s]+$", ErrorMessage = "No se permiten caracteres especiales o números.")]
	[Required(ErrorMessage = "Campo obligatorio.")]
	public string? Titulo { get; set; }

	[Required(ErrorMessage = "Campo obligatorio.")]
	[RegularExpression(@"[A-Za-z\s]+$", ErrorMessage = "No se permiten caracteres especiales o números.")]
	public string? Autores { get; set; }

	[Required(ErrorMessage = "Campo obligatorio.")]
	public string? Descripcion { get; set; }

	public double Precio { get; set; }

	public int Cantidad { get; set; }

	[Required(ErrorMessage = "Campo obligatorio.")]
	public DateTime FechaPublicacion { get; set; }

	[Required(ErrorMessage = "Campo obligatorio.")]
	public string? ImagenUrl { get; set; }


	// Relación con géneros
	public ICollection<DetalleLibroGenero> ListaGeneros { get; set; } = new List<DetalleLibroGenero>();
}

