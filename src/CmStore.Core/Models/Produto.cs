using System.ComponentModel.DataAnnotations;

namespace CmStore.Core.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Descricao { get; set; } = string.Empty;

    public string Imagem { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public int QuantidadeEstoque { get; set; }

    public bool Ativo { get; set; }

    //public Guid VendedorId { get; set; }
    //public Vendedor Vendedor { get; set; }

    //public Guid CategoriaId { get; set; }
    //public Categoria Categoria { get; set; }
}
