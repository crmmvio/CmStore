using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Ui.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nome,Ativo")] Categoria categoria)
        {
            ModelState.Remove("Produtos");

            if (ModelState.IsValid)
            {
                if(_context.UsingSqlLite)
                    categoria.Id = _context.Categorias.Max(e => e.Id) + 1;

                _context.Add(categoria);
                await _context.SaveChangesAsync();

                TempData.Add("Success", "Categoria cadastrada com sucesso!");
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nome,Ativo")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Produtos");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();

                    TempData.Add("Success", "Categoria alterada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!CategoriaExists(categoria.Id))
                    {
                        TempData.Add("Error", "Categoria não encotrada");
                    }
                    else
                    {
                        TempData.Add("Error", "Ocorreu um erro inesperado");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                                          .AsNoTracking()
                                          .Include(e=> e.Produtos)
                                          .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            if(categoria.Produtos != null && categoria.Produtos.Any())
            {
                TempData.Add("Warning", "Não é possível excluir a categoria pois existem produtos vinculados.");
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            TempData.Add("Success", "Categoria excluída com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
