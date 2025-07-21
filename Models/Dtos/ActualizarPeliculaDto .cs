﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models.Dtos
{
	public class ActualizarPeliculaDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string? RutaImagen { get; set; }

        public string? RutaLocalImagen { get; set; }

        public IFormFile Imagen { get; set; }

        public enum TipoClasificacion { Siete, Trece, Quince, Dieciocho }

        public TipoClasificacion Clasificacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public int CategoriaID { get; set; }

    }
}

