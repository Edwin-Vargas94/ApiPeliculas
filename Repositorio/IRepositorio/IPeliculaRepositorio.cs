using System;
using ApiPeliculas.Models;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        //V1
        //ICollection<Pelicula> GetPeliculas();

        //V2
        ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize);

        int GetTotalPeliculas();

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

