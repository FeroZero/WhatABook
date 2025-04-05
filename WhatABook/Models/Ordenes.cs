using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WhatABook.Enum;

namespace WhatABook.Models;

public class Ordenes
{
	[Key]
	public int OrdenId { get; set; }

	[ForeignKey("ApplicationUser")]
	public int UsuarioId { get; set; }

	[RegularExpression(@"[A-Za-z\sáéíóúÁÉÍÓÚñÑ]+$", ErrorMessage = "Caracter inválido")]
	[StringLength(70, ErrorMessage = "Límite Excedido.")]
	public string? NombreUsuario { get; set; }

	public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

	[ForeignKey("MetodoDePago")]
	public int MetodoDePagoId { get; set; }

	[Required(ErrorMessage = "Campo Obligatorio.")]
	public DateTime Fecha { get; set; }

	public double MontoTotal { get; set; }

	public ICollection<OrdenesDetalle> ordenes { get; set; } = new List<OrdenesDetalle>();

	public MetodosDePagos? MetodoDePago { get; set; }
}