using CmStore.Domain.Models.Base;
using CmStore.Domain.Models.Produtos;

namespace CmStore.Domain.Models.Categorias
{
    public class Categoria: EntityBase
    {
        public required string Codigo { get; set; }
        public required string Nome { get; set; }
        public bool Ativo { get; set; } 

        public IEnumerable<Produto> Produtos { get; set; }
    }
}
