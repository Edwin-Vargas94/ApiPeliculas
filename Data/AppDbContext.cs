using System;
using ApiCategorias.Models;
using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCategorias.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
		{
		}


		//Aquí pasar todas las entidades (Models)
		public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}