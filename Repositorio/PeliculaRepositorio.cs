using System;
using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
	public class PeliculaRepositorio : IPeliculaRepositorio

    {
		private readonly AppDbContext _bd;

        public PeliculaRepositorio(AppDbContext db)
		{
            _bd = db;
		}

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            // EGVG 020725 Arreglar problema de put
            //020725 EGVG: Actualiza una película existente o la agrega si no existe
            var PeliculaExistente = _bd.Pelicula.Find(pelicula.Id);

            //020725 EGVG: Si la película existe, actualiza sus valores
            if (PeliculaExistente != null)
            {
                _bd.Entry(PeliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                //020725 EGVG: Si no existe, la marca para actualizar (se comportará como inserción si es necesario)
                _bd.Pelicula.Update(pelicula);
            }

            //020725 EGVG: Guarda los cambios en la base de datos
            return Guardar();
        }

        //020725 EGVG: Elimina una película de la base de datos
        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);
            return Guardar(); //020725 EGVG: Guarda los cambios
        }

        //020725 EGVG: Busca películas por nombre o descripción
        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Pelicula;

            //020725 EGVG: Si el nombre no está vacío, filtra por coincidencias parciales en nombre o descripción
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }

            return query.ToList(); //020725 EGVG: Devuelve la lista de resultados
        }

        //020725 EGVG: Crea una nueva película y la guarda en la base de datos
        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now; //020725 EGVG: Asigna la fecha de creación actual
            _bd.Pelicula.Add(pelicula);
            return Guardar(); //020725 EGVG: Guarda los cambios
        }

        //020725 EGVG: Verifica si existe una película por Id
        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);
        }

        //020725 EGVG: Verifica si existe una película por nombre (normaliza a minúsculas y sin espacios extra)
        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre);
            return valor;
        }

        //020725 EGVG: Obtiene una película por su Id
        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == peliculaId);
        }

        //V1
        //public ICollection<Pelicula> GetPeliculas()
        //{
        //    return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        //}

        //V2
        //160725 EGVG: Obtiene una colección paginada de películas ordenadas por nombre
        public ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize)
        {
            //160725 EGVG: Ordena las películas alfabéticamente, omite las ya mostradas según la página
            //             y toma solo la cantidad especificada por pageSize
            return _bd.Pelicula.OrderBy(c => c.Nombre)
                .Skip((pageNumber - 1) * pageSize)  //160725 EGVG: Salta los registros de las páginas anteriores
                .Take(pageSize)                     //160725 EGVG: Toma los registros de la página actual
                .ToList();                          //160725 EGVG: Convierte el resultado a una lista
        }

        public int GetTotalPeliculas()
        {
            return _bd.Pelicula.Count();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int CatID)
        {
            return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.CategoriaID == CatID).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}

