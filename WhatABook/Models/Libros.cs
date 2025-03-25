using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class Libros
{
    [Key]

    public int LibroId { get; set; }

    [RegularExpression(@"[A-Za-z\s]+$", ErrorMessage = "No se permiten caracteres especiales o numeros")]
    [Required(ErrorMessage = " Campo obligatorio")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [RegularExpression(@"[A-Za-z\s]+$", ErrorMessage = "No se permiten caracteres especiales o numeros")]
    public string Autores { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public DateTime FechaPublicacion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public string ImagenUrl { get; set; }

    public virtual ICollection<Generos> Generos { get; set; } = new List<Generos>();


}

