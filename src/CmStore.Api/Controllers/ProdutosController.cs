using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public ProdutosController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    private async Task<IdentityUser> GetCurrentUserAsync() => await _userManager.FindByIdAsync(User.Identity.Name);

    /// <summary>
    /// Lista Todos os Produtos cadastrados
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }
        
        var produtos = await _context.Produtos.AsNoTracking()
                                              .Include(e=> e.Vendedor)
                                              .Include(e=> e.Categoria)
                                              .ToListAsync();
        return Ok(produtos);
    }

    /// <summary>
    /// Obtem Produto por Id
    /// </summary>
    /// <param name="id">Informe o Id do Produto</param>
    /// <returns></returns>
    [HttpGet("{id:int}")]    
    [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> GetById(int id)
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var user = await GetCurrentUserAsync();

        var produto = await _context.Produtos.AsNoTracking()
                                             .Include(e => e.Vendedor)
                                             .Include(e => e.Categoria)
                                             .FirstOrDefaultAsync(e=> e.Id == id && e.VendedorId == user.Id);
        if (produto == null)
        {
            return NotFound();
        }

        return Ok(produto);
    }

    /// <summary>
    /// Lista Produtos por Categoria
    /// </summary>
    /// <param name="id">Informe o Id da Categoria</param>
    /// <returns></returns>
    [HttpGet("categoria/{id:int}")]
    [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Produto>>> GetByCategory(int id) 
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produtos = await _context.Produtos.AsNoTracking()
                                              .Include(e => e.Vendedor)
                                              .Include(e => e.Categoria)
                                              .Where(e=> e.CategoriaId == id)
                                              .ToListAsync();
        return Ok(produtos);
    }

    /// <summary>
    /// Adiciona novo Produto
    /// </summary>
    /// <param name="produto">Informe os dados do Produto</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> Create([FromBody] Produto produto)
    {
        if (_context.Produtos == null || produto == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }

        var user = await GetCurrentUserAsync();
        produto.VendedorId = user.Id;

        ModelState.Remove("Categoria");
        ModelState.Remove("Vendedor");
        ModelState.Remove("VendedorId");

        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });
        }
        
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    /// <summary>
    /// Atualiza dados do Produto
    /// </summary>
    /// <param name="id">Informe o Id do Produto</param>
    /// <param name="produto">Informe os dados do Produto</param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> Update(int id, [FromBody] Produto produto)
    {
        if (_context.Produtos == null)
        {
            return Problem("Erro ao atualizar um produto, contate o suporte!");
        }

        var user = await GetCurrentUserAsync();

        if (id != produto.Id || produto.VendedorId != user.Id) return BadRequest();

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!ProdutoExists(id))
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
    /// Exluir registro de Produto
    /// </summary>
    /// <param name="id">Informe o Id do Produto</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (_context.Produtos == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }

        var user = await GetCurrentUserAsync();
        var produto = await _context.Produtos.FirstOrDefaultAsync(e=> e.Id == id && e.VendedorId == user.Id);
        if (produto == null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProdutoExists(int id)
    {
        return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
