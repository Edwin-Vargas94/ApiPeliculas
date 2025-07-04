using System;
using ApiCategorias.Models;

namespace ApiCategorias.Repositorio.IRepositorio
{
	public interface ICategoriaRepositorio
	{
		ICollection<Categoria> GetCategorias();
		Categoria GetCategoria(int CategoriaID);
		bool ExisteCategoria(int ID);
        bool ExisteCategoria(string nombre);

        bool CrearCategoria(Categoria Categoria);

        bool ActualizarCategoria(Categoria Categoria);

        bool BorrarCategoria(Categoria Categoria);

        bool Guardar();
    }
}

