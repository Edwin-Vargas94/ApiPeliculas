using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
	public class CategoriaDto
	{
        public int ID { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(60, ErrorMessage = "El número máximo de carácteres es de 60!")]
        public string Nombre { get; set; }

        //[Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
	}
}

