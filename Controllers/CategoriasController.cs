using Microsoft.AspNetCore.Mvc;
using ApiCategorias.Models; // o el namespace correcto donde estén tus modelos
using ApiCategorias.Models.Dtos; // donde estén tus DTOs
using ApiCategorias.Repositorio.IRepositorio; // donde esté la interfaz del repositorio
using AutoMapper;

namespace ApiCategorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;   
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status200OK)]

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

        [HttpGet("{CategoriaID:int}", Name = "GetCategoria")]
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


                var Categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_ctRepo.CrearCategoria(Categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{Categoria.Nombre}");
                    return StatusCode(404, ModelState);
            }
                return CreatedAtRoute("GetCategoria", new {CategoriaID = Categoria.ID}, Categoria);
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

            var Categoria = _mapper.Map<Categoria>(CategoriaDto);

            if (!_ctRepo.ActualizarCategoria(Categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{Categoria.Nombre}");
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

        public IActionResult ActualizarPutCategoria(int CategoriaID, [FromBody] CategoriaDto CategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CategoriaDto == null || CategoriaID != CategoriaDto.ID)
            {
                return BadRequest(ModelState);
            }

            var CategoriaExistente = _mapper.Map<Categoria>(CategoriaDto);

            if (CategoriaExistente == null)
            {
                return NotFound($"No se encontro la Categoria con ID {CategoriaID}");
            }

            var Categoria = _mapper.Map<Categoria>(CategoriaDto);

            if (!_ctRepo.ActualizarCategoria(Categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{Categoria.Nombre}");
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

        public IActionResult ActualizarPutCategoria(int CategoriaID)
        {

            if (!_ctRepo.ExisteCategoria(CategoriaID))  
            {
                return NotFound();
            }

            var Categoria = _ctRepo.GetCategoria(CategoriaID);

            if (!_ctRepo.BorrarCategoria(Categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro{Categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
