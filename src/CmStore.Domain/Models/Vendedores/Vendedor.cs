using CmStore.Domain.Models.Base;
using CmStore.Domain.Models.Produtos;

namespace CmStore.Domain.Models.Vendedores
{
    public class Vendedor : EntityBase
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<Produto> Produtos { get; set; }
    }
}
