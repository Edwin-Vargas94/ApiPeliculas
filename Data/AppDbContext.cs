using System;
using ApiPeliculas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
	public class AppDbContext : IdentityDbContext<AppUsuario>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
		{
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Aquí pasar todas las entidades (Models)
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pelicula> Pelicula { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}