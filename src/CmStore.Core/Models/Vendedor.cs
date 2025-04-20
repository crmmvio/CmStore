namespace CmStore.Core.Models;

public class Vendedor
{
    public string Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public bool Ativo { get; set; }

    public virtual IEnumerable<Produto> Produtos { get; set; }
}
