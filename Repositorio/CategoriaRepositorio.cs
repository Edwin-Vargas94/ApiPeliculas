using System;
using ApiCategorias.Data;
using ApiCategorias.Models;
using ApiCategorias.Repositorio.IRepositorio;

namespace ApiCategorias.Repositorio
{
	public class CategoriaRepositorio : ICategoriaRepositorio

	{
		private readonly AppDbContext _bd;

        public CategoriaRepositorio(AppDbContext db)
		{
            _bd = db;
		}

        public bool ActualizarCategoria(Categoria Categoria)
        {
            Categoria.FechaCreacion = DateTime.Now;
            // EGVG 020725 Arreglar problema de put
            var CategoriaExistente = _bd.Categorias.Find(Categoria.ID);
            if (CategoriaExistente != null)
            {
                _bd.Entry(CategoriaExistente).CurrentValues.SetValues(Categoria);
            }
            else
            {
                _bd.Categorias.Update(Categoria);
            }

            return Guardar();
        }

        public bool BorrarCategoria(Categoria Categoria)
        {
            _bd.Categorias.Remove(Categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria Categoria)
        {
            Categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Add(Categoria);
            return Guardar();
        }

        public bool ExisteCategoria(int id)
        {
            return _bd.Categorias.Any(c => c.ID == id);
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre);
            return valor;
        }

        public Categoria GetCategoria(int CategoriaID)
        {
            return _bd.Categorias.FirstOrDefault(c => c.ID == CategoriaID);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}

