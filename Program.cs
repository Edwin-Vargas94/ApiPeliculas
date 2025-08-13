using System;
using System.Linq;
using System.Text;
using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ====== DbContext (SQL Server desde env o appsettings) ======
var connectionString =
    Environment.GetEnvironmentVariable("ConnectionSql") ??
    builder.Configuration.GetConnectionString("ConnectionSql");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ====== Identity + Roles ======
builder.Services.AddIdentity<AppUsuario, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// ====== Versionado de API ======
var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
});

apiVersioningBuilder.AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

// ====== Repositorios ======
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// ====== Cache ======
builder.Services.AddResponseCaching();

// ====== AutoMapper ======
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

// ====== JWT Bearer ======
var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
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

// ====== Controllers (con perfil de cache global de ejemplo) ======
builder.Services.AddControllers(opcion =>
{
    opcion.CacheProfiles.Add("PorDefecto30Segundos", new CacheProfile { Duration = 30 });
});

// ====== Swagger ======
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Autenticación JWT usando el esquema Bearer.\r\n" +
        "Ejemplo: \"Bearer AQUI_TU_TOKEN\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new string[]{}
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
    });

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
    });
});

// ====== CORS (dinámico por variable de entorno + match flexible) ======
var corsOriginsRaw = Environment.GetEnvironmentVariable("CORS_ORIGINS");
Console.WriteLine("CORS_ORIGINS raw: " + corsOriginsRaw);

var corsOrigins = corsOriginsRaw?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(o => o.Trim().TrimEnd('/').ToLowerInvariant())
    .ToArray();

if (corsOrigins is not null && corsOrigins.Any())
{
    Console.WriteLine("Orígenes permitidos (normalizados):");
    foreach (var origin in corsOrigins)
    {
        Console.WriteLine($"- '{origin}'");
    }
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors", build =>
    {
        if (corsOrigins is not null && corsOrigins.Any())
        {
            build
                .SetIsOriginAllowed(origin =>
                {
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    var clean = origin.Trim().TrimEnd('/').ToLowerInvariant();
                    return corsOrigins.Contains(clean);
                })
                .AllowAnyMethod()
                .AllowAnyHeader();
            // .AllowCredentials(); // habilítalo solo si usas cookies/autenticación basada en cookies
        }
        else
        {
            // Fallback para desarrollo/local si no hay variable
            build.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
        }
    });
});

var app = builder.Build();

// ====== Swagger UI ======
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculasV1");
        o.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculasV2");
    });
}
else
{
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("https://apipeliculas-production-c545.up.railway.app/swagger/v1/swagger.json", "ApiPeliculasV1");
        o.SwaggerEndpoint("https://apipeliculas-production-c545.up.railway.app/swagger/v2/swagger.json", "ApiPeliculasV2");
        o.RoutePrefix = string.Empty; // Swagger en raíz
    });
}

// ====== Middlewares ======
app.UseStaticFiles();
// app.UseHttpsRedirection(); // Actívalo si configuras forwarded headers/proxy

app.UseRouting();

app.UseCors("PoliticaCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ====== Puerto dinámico para Railway ======
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Clear();
    app.Urls.Add($"http://0.0.0.0:{port}");
    Console.WriteLine($"Escuchando en puerto Railway: {port}");
}

app.Run();