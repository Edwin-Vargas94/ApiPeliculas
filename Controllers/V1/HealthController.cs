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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var count = await _db.Pelicula.CountAsync();
                return Ok(new
                {
                    status = "OK",
                    message = "Conexión exitosa a la base de datos.",
                    peliculasRegistradas = count
                });
            }
            catch (Exception ex)
            {
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