using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Ui.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProdutosController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<IdentityUser> GetUsuarioLogado()
        {
            var user = await _userManager.GetUserAsync(User);

            return user; 
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUsuarioLogado();
            var appDbContext = _context.Produtos
                                       .Include(p => p.Categoria)
                                       .Include(p => p.Vendedor)
                                       .Where(e=> e.VendedorId == user.Id);

            return View(await appDbContext.ToListAsync());
        }

        [HttpGet()]
        public async Task<IActionResult> Details(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        public IActionResult Create()
        {
            var produto = new Produto() { Ativo = true };
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Imagem,Preco,QuantidadeEstoque,CategoriaId,Ativo")] Produto produto)
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            var vendedor = await GetUsuarioLogado();

            if (vendedor == null)
            {
                ModelState.AddModelError("", "Vendedor não encontrado");
                return View(produto);
            }

            produto.VendedorId = vendedor.Id;

            ModelState.Remove("VendedorId");
            ModelState.Remove("Vendedor");
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Imagem,Preco,QuantidadeEstoque,VendedorId,CategoriaId,Ativo")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            ModelState.Remove("VendedorId");
            ModelState.Remove("Vendedor");
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpGet()]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost(), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
