	using System;
	using System.ComponentModel.DataAnnotations;

namespace ApiCategorias.Models
	{
		public class Categoria
		{
			[Key]
			public int ID { get; set; }

			[Required]
			public string Nombre { get; set; }

			public string? Descripcion { get; set; }

			[Required]
			//[Display(Name = "Fecha de creación")]
			public DateTime FechaCreacion { get; set; }

		}
	}

