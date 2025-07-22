using Microsoft.AspNetCore.Mvc;
using ApiPeliculas.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _db;

        public HealthController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Verifica la conexión con la base de datos en Azure.
        /// </summary>
        /// <returns>JSON con estado y número de películas registradas.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Entrando al endpoint /api/health");
            try
            {
                Console.WriteLine("Verificando si DbContext está funcionando...");

                var conectado = await _db.Database.CanConnectAsync();
                Console.WriteLine($"¿Puede conectar? {conectado}");

                var count = await _db.Pelicula.CountAsync();
                Console.WriteLine($"Películas registradas: {count}");

                return Ok(new
                {
                    status = "OK",
                    message = "Conexión exitosa a la base de datos.",
                    peliculasRegistradas = count
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en /api/health: {ex.Message}");
                return StatusCode(500, new
                {
                    status = "ERROR",
                    message = "No se pudo conectar a la base de datos.",
                    detalles = ex.Message
                });
            }
        }

    }
}