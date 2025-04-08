using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendedorController : ControllerBase
    {
        ILogger<VendedorController> _logger;

        public VendedorController(ILogger<VendedorController> logger)
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
