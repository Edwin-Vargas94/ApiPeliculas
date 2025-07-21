using Microsoft.AspNetCore.Mvc;
using ApiPeliculas.Repositorio.IRepositorio; // donde esté la interfaz del repositorio
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Models;
using Asp.Versioning;

namespace ApiPeliculas.Controllers.V1
{
    //[Route("api/[controller]")] //Opción estática
    //[Authorize(Roles = "Admin")]
    //[ResponseCache(Duration = 20)]
    [Route("api/v{version:apiVersion}/categorias")] //Opción dinamicá
    [ApiController]
    [ApiVersion("1.0")]
    //[Obsolete("Esta versión del controlador esta obsoleta")]

    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;   
        }

        [HttpGet("GetString")]
        [Obsolete("Esta enpoint esta obsoleto")]
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Valor1", "Valor2", "Valor3" };
        }

        [AllowAnonymous]
        [HttpGet]
        //[ResponseCache(Duration = 20)]
        [ResponseCache(CacheProfileName = "PorDefecto30Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[EnableCors("PoliticaCors")] //Aplica la pólitica CORS a este method.

        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }

        [AllowAnonymous]
        [HttpGet("{CategoriaID:int}", Name = "GetCategoria")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ResponseCache(CacheProfileName = "PorDefecto30Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetCategoria(int CategoriaID)
        {
            var itemCategoria = _ctRepo.GetCategoria(CategoriaID);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CrearCategoria([FromBody]CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                    return BadRequest(ModelState);
            }

            if (crearCategoriaDto == null)
            {
                    return BadRequest(ModelState);  
            }

            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", $"La Categoria ya existe");
                    return StatusCode(404, ModelState);
            }


                var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
                    return StatusCode(404, ModelState);
            }
                return CreatedAtRoute("GetCategoria", new {CategoriaID = categoria.ID}, categoria);
        }

        [HttpPatch("{CategoriaID:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult ActualizarPatchCategoria(int CategoriaID, [FromBody] CategoriaDto CategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CategoriaDto == null || CategoriaID != CategoriaDto.ID)
            {
                return BadRequest(ModelState);
            }

            var CategoriaExistente = _ctRepo.GetCategoria(CategoriaID);

            if (CategoriaExistente == null)
            {
                return NotFound($"No se encontro la Categoria con ID {CategoriaID}");
            }

            var categoria = _mapper.Map<Categoria>(CategoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpPut("{CategoriaID:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult ActualizarPutCategoria(int categoriaId, [FromBody] CategoriaDto CategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CategoriaDto == null || categoriaId != CategoriaDto.ID)
            {
                return BadRequest(ModelState);
            }

            var CategoriaExistente = _mapper.Map<Categoria>(CategoriaDto);

            if (CategoriaExistente == null)
            {
                return NotFound($"No se encontro la Categoria con ID {categoriaId}");
            }

            var categoria = _mapper.Map<Categoria>(CategoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{CategoriaID:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult BorrarCategoria(int categoriaId)
        {

            if (!_ctRepo.ExisteCategoria(categoriaId))  
            {
                return NotFound();
            }

            var categoria = _ctRepo.GetCategoria(categoriaId);

            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
