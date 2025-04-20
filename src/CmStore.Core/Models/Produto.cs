using System.ComponentModel.DataAnnotations;

namespace CmStore.Core.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    public string Imagem { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Display(Name = "Qtde Estoque")]
    public int QuantidadeEstoque { get; set; }

    public bool Ativo { get; set; }

    //[Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string VendedorId { get; set; }
    public virtual Vendedor Vendedor { get; set; }

    //[Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }

    public virtual Categoria Categoria { get; set; }
}
