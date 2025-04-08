using CmStore.Data.Contexts;
using CmStore.Domain.Models.Categorias;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context): base(context) { }

        public async Task<Categoria> ObterCategoriaProduto(Guid id, bool tracking)
        {
            var queryResult = DbContext.Categorias.Include(f => f.Produtos);

            if (!tracking)
                queryResult.AsNoTracking();

            return await queryResult.FirstOrDefaultAsync(e=>e.Id == id);
        }

        public async Task<IEnumerable<Categoria>> ObterCategoriasProdutos()
        {
            var queryResult = DbContext.Categorias.AsNoTracking()
                                                  .Include(f => f.Produtos);

            return await queryResult.ToListAsync();
        }
    }
}
