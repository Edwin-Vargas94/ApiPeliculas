using ApiPeliculas.Data;
using Microsoft.EntityFrameworkCore;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ApiPeliculas.PeliculasMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using ApiPeliculas.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<AppDbContext>(opciones =>
//opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSql")));
var connectionString = Environment.GetEnvironmentVariable("ConnectionSql");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//130725 EGVG: Soporte para autentificación con .NET Identity.
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

//130725 EGVG: Soporte para cache.
var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
    //opcion.ApiVersionReader = ApiVersionReader.Combine(
      //  new QueryStringApiVersionReader("api-version")//?api-version=1.0
        //new HeaderApiVersionReader("X-version"),
        //new MediaTypeApiVersionReader("ver"));
    //);
});

apiVersioningBuilder.AddApiExplorer(
    opciones =>
    {
        opciones.GroupNameFormat = "'v'VVV";
        opciones.SubstituteApiVersionInUrl = true;
    }
    );

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//140725 EGVG: Sopporte para versionamiento
builder.Services.AddResponseCaching();

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Agregamos el AutoMapper
//builder.Services.AddAutoMapper(typeof(CategoriasMapper)); // ✅ ESTA ES LA CORRECTA

//  Aquí se configura la autentificación.
builder.Services.AddAuthentication
    (
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };


    });
    

builder.Services.AddControllers(opcion =>
{
    //cache profile. Un cachhe global y asi no tener que ponerlo en todas partes
    opcion.CacheProfiles.Add("PorDefecto30Segundos", new CacheProfile() { Duration = 30 });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra 'Bearer' seguido de un espacio y despues el token en el campo de abajo \r\n\r\n " +
        "Ejemplo: \"Bearer ashyaujsik\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name= "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
            Version = "v1.0",
            Title = "Peliculas Api V1",
            TermsOfService = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/"),
            Contact = new OpenApiContact
            {
                Name = "DevEGVGDotNet",
                Url = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/")
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/")
            }
    }

    );
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Peliculas Api V2",
        TermsOfService = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/"),
        Contact = new OpenApiContact
        {
            Name = "DevEGVGDotNet",
            Url = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            Url = new Uri("https://www.linkedin.com/in/edwin-vargas-993691129/")
        }
    }

    );
});

//Soporte para CORS
//Se pueden habilitar: 1-Dominio, 2-multiples dominios,
//3-cualquier dominio (Tener en cuenta seguridad)
//Usamos de ejemplo del dominio: http://localhost:3223, se debe cambiar por el correcto
//Se usa (*) para alls dominios.
builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("http://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();
app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculasV2");
    });
}
else
{
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculasV2");
        opciones.RoutePrefix = "";
    });
}

//Soporte para archivos estaticos como imagenes
app.UseStaticFiles();
app.UseHttpsRedirection();

//Soporte para CORS
//app.UseCors("PoliticaCors");

//Soporte para Autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

