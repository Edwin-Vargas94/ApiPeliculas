﻿using System;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
	public interface IUsuarioRepositorio
    {
		ICollection<AppUsuario> GetUsuarios();

		AppUsuario GetUsuario(string usuarioId);

		bool IsUniqueUser(string usuario);

		Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}