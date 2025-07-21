using Microsoft.AspNetCore.Mvc;
using ApiPeliculas.Repositorio.IRepositorio; // donde esté la interfaz del repositorio
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiPeliculas.Controllers
{
    //[Route("api/[controller]")] //Opción estática
    //[Authorize(Roles = "Admin")]
    //[ResponseCache(Duration = 20)]
    [Route("api/v{version:apiVersion}/categorias")] //Opción dinamicá
    [ApiController]
    [ApiVersion("2.0")]

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
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Edwin", "Vargas", "DotNet" };
        }
    }
}
