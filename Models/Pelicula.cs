using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCategorias.Models
{
	public class Pelicula
	{
		[Key]
		public int Id { get; set; }
			
		public string Nombre { get; set; }

		public string Descripcion { get; set; }

		public int Duracion { get; set; }

		public string RutaImagen { get; set; }

		public enum TipoClasificacion { Siete, Trece, Quince, Dieciocho }

		public TipoClasificacion Clasificacion { get; set; }

		public DateTime FechaCreacion { get; set; }


		//Relación con Categoria

		public int CategoriaID { get; set; }
		[ForeignKey("CategoriaID")]

		public Categoria Categoria { get; set; }
	
	}
}

