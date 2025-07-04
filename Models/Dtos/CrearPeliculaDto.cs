using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCategorias.Models.Dtos
{
	public class CrearPeliculaDto
    {

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string RutaImagen { get; set; }

        public enum CrearTipoClasificacion { Siete, Trece, Quince, Dieciocho }

        public CrearTipoClasificacion Clasificacion { get; set; }

        public int CategoriaID { get; set; }

    }
}

