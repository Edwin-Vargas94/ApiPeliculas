﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
	public class UsuarioLoginDto
	{
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; }
    }
}

