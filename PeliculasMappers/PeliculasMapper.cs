﻿using System;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
	public class PeliculasMapper : Profile
	{
		public PeliculasMapper()
		{
			CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<Pelicula, CrearPeliculaDto>().ReverseMap();
            CreateMap<Pelicula, ActualizarPeliculaDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
            //CreateMap<Usuario, UsuarioDto>().ReverseMap();

            //using ApiCategorias.Models;
            //using ApiCategorias.Models.Dtos;
        }
    }
}

