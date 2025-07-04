using System;
using ApiCategorias.Models;

namespace ApiCategorias.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
		ICollection<Pelicula> GetPeliculas();

        ICollection<Pelicula> GetPeliculasEnCategoria(int CatID);

        IEnumerable<Pelicula> BuscarPelicula(string nombre);

        Pelicula GetPelicula(int PeliculaId);

		bool ExistePelicula(int ID);

        bool ExistePelicula(string nombre);

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);

        bool Guardar();
    }
}

