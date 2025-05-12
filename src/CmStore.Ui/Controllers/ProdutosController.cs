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
            var dataQuery = _context.Produtos
                                    .AsNoTracking()
                                    .Include(p => p.Categoria)
                                    .Include(p => p.Vendedor)
                                    .Where(e => e.VendedorId == user.Id);

            var result = await dataQuery.ToListAsync();

            return View(result);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(e=> e.Ativo), "Id", "Nome");

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,ImagemUpload,Preco,QuantidadeEstoque,CategoriaId,Ativo")] Produto produto)
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
                var imgPrefixo = Guid.NewGuid().ToString() + "_";
                if (produto.ImagemUpload != null)
                {
                    var resultado = await UploadArquivo(produto.ImagemUpload, imgPrefixo);
                    if (resultado)
                    {
                        produto.Imagem = imgPrefixo + produto.ImagemUpload.FileName;
                    }
                }

                if (_context.UsingSqlLite)
                    produto.Id = _context.Produtos.Max(e => e.Id) + 1;

                _context.Add(produto);
                await _context.SaveChangesAsync();
                TempData.Add("Success", "Produto cadastrado com sucesso!");

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,ImagemUpload,Preco,QuantidadeEstoque,VendedorId,CategoriaId,Ativo")] Produto produto)
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
                var produtoDb = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produto.Id);
                produto.Imagem = produtoDb != null ? produtoDb.Imagem : string.Empty;

                if (produto.ImagemUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid().ToString() + "_";
                    var resultado = await UploadArquivo(produto.ImagemUpload, imgPrefixo);
                    if (resultado)
                    {
                        produto.Imagem = imgPrefixo + produto.ImagemUpload.FileName;
                    }
                }

                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();

                    TempData.Add("Error", "Produto atualizado com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        TempData.Add("Error", "Produto não encontrado");
                    }
                    else
                    {
                        TempData.Add("Error", "Ocorreu um erro inesperado ao tentar editar o produto.");
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
            TempData.Add("Success", "Produto excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            var dirImages = "wwwroot/imagens";
            if (arquivo.Length <= 0) return false;

            if (!Directory.Exists(dirImages))
                Directory.CreateDirectory(dirImages);

            var path = Path.Combine(Directory.GetCurrentDirectory(), dirImages, imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError("", "Já existe um arquivo com esse nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
