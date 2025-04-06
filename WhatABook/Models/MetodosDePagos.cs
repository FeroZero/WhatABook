using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models
{
	public class MetodosDePagos
	{
		[Key]
		public int MetodoPagoId { get; set; }

		[Required(ErrorMessage = "Campo Obligatorio")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El Nombre debe contener solo letras.")]
		public string? Descripcion { get; set; }
	}
}
