using CmStore.Data.Contexts;
using CmStore.Domain.Models.Produtos;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Data.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> ObterProdutosPorVendedor(Guid vendedorId)
        {
            var queryResult = DbContext.Produtos.AsNoTracking()
                                                .Include(f => f.Vendedor)
                                                .Include(f => f.Categoria)
                                                .Where(e=> e.VendedorId == vendedorId);

            return await queryResult.ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosVendedores()
        {
            var queryResult = DbContext.Produtos.AsNoTracking()
                                                .Include(f => f.Vendedor)
                                                .Include(f => f.Categoria);

            return await queryResult.OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<Produto> ObterProdutoVendedor(Guid id, bool tracking)
        {
            var queryResult = DbContext.Produtos.Include(f => f.Vendedor)
                                                .Include(f => f.Categoria);

            if (!tracking)
                queryResult.AsNoTracking();

            return await queryResult.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
