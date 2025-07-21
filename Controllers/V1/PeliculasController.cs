using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Repositorio.IRepositorio;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiPeliculas.Controllers
{
    [Route("api/v{version:apiVersion}/peliculas")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]


    public class PeliculasController : ControllerBase
    {
        //140725 EGVG: Inyección de dependencias del repositorio y del mapper
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        //140725 EGVG: Constructor del controlador
        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        ////140725 EGVG: Endpoint para obtener todas las películas (sin autenticación)
        //[AllowAnonymous]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public IActionResult GetPeliculas()
        //{
        //    //140725 EGVG: Obtiene todas las películas del repositorio
        //    var listaPeliculas = _pelRepo.GetPeliculas();

        //    //140725 EGVG: Inicializa la lista DTO para devolver al cliente
        //    var listaPeliculasDto = new List<PeliculaDto>();

        //    //140725 EGVG: Mapea cada película a su DTO
        //    foreach (var lista in listaPeliculas)
        //    {
        //        listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
        //    }

        //    //140725 EGVG: Devuelve la lista de películas con estado 200
        //    return Ok(listaPeliculasDto);
        //}


        //V2 170725 EGVG: Endpoint para obtener todas las películas (sin autenticación)
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetPeliculas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {

            try
            {
                var totalPeliculas = _pelRepo.GetTotalPeliculas();
                var peliculas = _pelRepo.GetPeliculas(pageNumber, pageSize);

                if (peliculas == null || !peliculas.Any())
                {
                    return NotFound("No se encontraron películas");
                }

                var peliculasDto = peliculas.Select(p => _mapper.Map<PeliculaDto>(p)).ToList();

                var response = new
                {
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalPeliculas / (double)pageSize),
                    TotalItems = totalPeliculas,
                    Items = peliculasDto
                };
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }


        //140725 EGVG: Endpoint para obtener una película por su Id (sin autenticación)
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            //140725 EGVG: Busca la película por Id
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);

            //140725 EGVG: Si no existe, devuelve 404
            if (itemPelicula == null)
            {
                return NotFound();
            }

            //140725 EGVG: Mapea la entidad a su DTO
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);

            //140725 EGVG: Devuelve la película encontrada con estado 200
            return Ok(itemPeliculaDto);
        }

        //140725 EGVG: method HTTP POST para crear una película
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromForm] CrearPeliculaDto crearPeliculaDto)
        {
            //140725 EGVG: Valida si el modelo recibido no es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //140725 EGVG: Verifica que el DTO no sea nulo
            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            //140725 EGVG: Comprueba si ya existe una película con el mismo nombre
            if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", $"La película ya existe");
                return StatusCode(404, ModelState);
            }

            //140725 EGVG: Mapea el DTO recibido a la entidad de dominio Pelicula
            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

            //  if (!_pelRepo.CrearPelicula(pelicula))
            //  {
            //      ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
            //      return StatusCode(404, ModelState);
            //  }
            // return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);

            //160725 EGVG: Subida de Archivo

            //160725 EGVG: Verifica si se envió una imagen en el DTO
            if (crearPeliculaDto.Imagen != null)
            {
                //160725 EGVG: Genera un nombre único para el archivo usando el Id de la película y un GUID
                string nombreArchivo = pelicula.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(crearPeliculaDto.Imagen.FileName);

                //160725 EGVG: Define la ruta relativa donde se guardará la imagen
                string rutaArchivo = @"wwwroot\ImagenesPeliculas\" + nombreArchivo;

                //160725 EGVG: Obtiene la ruta física completa en el servidor
                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

                FileInfo file = new FileInfo(ubicacionDirectorio);

                //160725 EGVG: Si ya existe un archivo con el mismo nombre, lo elimina
                if (file.Exists)
                {
                    file.Delete();
                }

                //160725 EGVG: Copia la imagen enviada al servidor en la ruta especificada
                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    crearPeliculaDto.Imagen.CopyTo(fileStream);
                }

                //160725 EGVG: Construye la URL pública de la imagen para acceder desde el cliente
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaImagen = baseUrl + "/ImagenesPeliculas/" + nombreArchivo;

                //160725 EGVG: Guarda también la ruta local del archivo en el servidor
                pelicula.RutaLocalImagen = rutaArchivo;
            }
            else
            {
                //160725 EGVG: Si no se envió una imagen, usa una imagen por defecto
                pelicula.RutaImagen = "https://placehold.co/600x400";
            }

            //160725 EGVG: Guarda la película en la base de datos
            _pelRepo.CrearPelicula(pelicula);

            //160725 EGVG: Devuelve la respuesta con la ruta a la película creada
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromForm] ActualizarPeliculaDto actualizarPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarPeliculaDto == null || peliculaId != actualizarPeliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var PeliculaExistente = _pelRepo.GetPelicula(peliculaId);

            if (PeliculaExistente == null)
            {
                return NotFound($"No se encontro la pelicula con ID {peliculaId}");
            }

            var pelicula = _mapper.Map<Pelicula>(actualizarPeliculaDto);

            // if (!_pelRepo.ActualizarPelicula(pelicula))
            // {
            //     ModelState.AddModelError("", $"Algo salio mal actualizando el registro{pelicula.Nombre}");
            //     return StatusCode(500, ModelState);
            // }

            //160725 EGVG: Subida de Archivo

            //160725 EGVG: Verifica si se envió una imagen en el DTO
            if (actualizarPeliculaDto.Imagen != null)
            {
                //160725 EGVG: Genera un nombre único para el archivo usando el Id de la película y un GUID
                string nombreArchivo = pelicula.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(actualizarPeliculaDto.Imagen.FileName);

                //160725 EGVG: Define la ruta relativa donde se guardará la imagen
                string rutaArchivo = @"wwwroot\ImagenesPeliculas\" + nombreArchivo;

                //160725 EGVG: Obtiene la ruta física completa en el servidor
                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

                FileInfo file = new FileInfo(ubicacionDirectorio);

                //160725 EGVG: Si ya existe un archivo con el mismo nombre, lo elimina
                if (file.Exists)
                {
                    file.Delete();
                }

                //160725 EGVG: Copia la imagen enviada al servidor en la ruta especificada
                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    actualizarPeliculaDto.Imagen.CopyTo(fileStream);
                }

                //160725 EGVG: Construye la URL pública de la imagen para acceder desde el cliente
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaImagen = baseUrl + "/ImagenesPeliculas/" + nombreArchivo;

                //160725 EGVG: Guarda también la ruta local del archivo en el servidor
                pelicula.RutaLocalImagen = rutaArchivo;
            }
            else
            {
                //160725 EGVG: Si no se envió una imagen, usa una imagen por defecto
                pelicula.RutaImagen = "https://placehold.co/600x400";
            }

            _pelRepo.ActualizarPelicula(pelicula);
            return NoContent();
        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult BorrarPelicula(int peliculaId)
        {

            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);

            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{CategoriaID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetPeliculasEnCategoria(int CategoriaID)
        {

        try
            {
                var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(CategoriaID);

                if (listaPeliculas == null || !listaPeliculas.Any())
                {
                    return NotFound($"No se encontraron péliculas en la categoria con ID {CategoriaID}");
                }

                var itemPelicula = listaPeliculas.Select(pelicula => _mapper.Map<PeliculaDto>(pelicula)).ToList();
                // foreach (var pelicula in listaPeliculas)
                // {
                //     itemPelicula.Add(_mapper.Map<PeliculaDto>(pelicula));
                // }

                return Ok(itemPelicula);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error, recuperando datos de la app.");
            }
        }

        [AllowAnonymous]
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Buscar(string nombre)
        {

            try
            {
                var peliculas = _pelRepo.BuscarPelicula(nombre);
                if (!peliculas.Any())
                {
                    return NotFound($"No se encontraron películas que coincidan con los criterios de busqueda");
                }

                var peliculasDto = _mapper.Map<IEnumerable<PeliculaDto>>(peliculas);
                return Ok(peliculasDto);
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }
    }
}