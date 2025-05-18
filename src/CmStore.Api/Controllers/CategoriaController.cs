using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Api.Controllers;

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

    /// <summary>
    /// Lista todas Categorias
    /// </summary>
    /// <returns></returns>
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

        var categorias = await _context.Categorias.AsNoTracking()
                                                  .Include(e=> e.Produtos)
                                                  .ToListAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtem Categoria por Id
    /// </summary>
    /// <param name="id">Informe o Id da Categoria</param>
    /// <returns></returns>
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

        var categoria = await _context.Categorias.AsNoTracking()
                                                 .Include(e => e.Produtos)
                                                 .FirstOrDefaultAsync(e=> e.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    /// <summary>
    /// Adiciona nova Categoria
    /// </summary>
    /// <param name="categoria">Informe os dados da Categoria</param>
    /// <returns></returns>
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

        if (_context.UsingSqlLite)
            categoria.Id = _context.Categorias.Max(e => e.Id) + 1;

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    /// <summary>
    /// Atualiza dados da Categoria informada
    /// </summary>
    /// <param name="id">Informe o Id Categoria</param>
    /// <param name="categoria">Informe os dados da Categoria</param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
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
        catch (DbUpdateConcurrencyException)
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

    /// <summary>
    /// Exclui regsistro de Categoria
    /// </summary>
    /// <param name="id">Informe o Id da Categoria</param>
    /// <returns></returns>
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

        var emUso = await _context.Produtos.AnyAsync(e=> e.CategoriaId == id);

        if (emUso)
        {
            return BadRequest("Categoria possuí produtos associados, não pode ser excluído.");
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
