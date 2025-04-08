using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(ILogger<ProdutosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }
    }
}
