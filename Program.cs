using ApiCategorias.Data;
using Microsoft.EntityFrameworkCore;
using ApiCategorias.Repositorio;
using ApiCategorias.Repositorio.IRepositorio;
using ApiCategorias.CategoriasMapper;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opciones =>
opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSql")));

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(CategoriasMapper)); // ✅ ESTA ES LA CORRECTA

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

