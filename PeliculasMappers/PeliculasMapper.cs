using System;
using ApiCategorias.Models;
using ApiCategorias.Models.Dtos;
using AutoMapper;

namespace ApiCategorias.CategoriasMapper
{
	public class CategoriasMapper : Profile
	{
		public CategoriasMapper()
		{
			CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<Pelicula, CrearPeliculaDto>().ReverseMap();
        }	
	}
}

