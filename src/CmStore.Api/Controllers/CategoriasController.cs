using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lista todas as categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get()
        {
            return Ok();
        }

        /// <summary>
        /// Retorna uma categoria específica
        /// </summary>
        /// <param name="id">Informe o Id da Categoria</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Adicionar()
        {
            return Ok();
        }

    }
}
