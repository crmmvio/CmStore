using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            var categorias = await _context.Categorias.ToListAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Categoria>> Create([FromBody] Categoria categoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            ModelState.Remove("Produtos");

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Categoria>> Update(int id, [FromBody] Categoria categoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            if (id != categoria.Id) return BadRequest();

            ModelState.Remove("Produtos");

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return (_context.Categorias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
