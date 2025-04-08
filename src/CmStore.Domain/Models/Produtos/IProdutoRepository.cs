using CmStore.Domain.Models.Base;

namespace CmStore.Domain.Models.Produtos
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorVendedor(Guid vendedorId);
        Task<IEnumerable<Produto>> ObterProdutosVendedores();
        Task<Produto> ObterProdutoVendedor(Guid id, bool tracking);
    } 
}
