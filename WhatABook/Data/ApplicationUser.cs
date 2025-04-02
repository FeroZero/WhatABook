using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		public string? Nombre { get; set; }

        public int? Edad { get; set; }

		public string? Direccion { get; set; }

	}

}
