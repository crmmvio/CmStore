using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Display(Name = "Imagem do Produto")]
    public string Imagem { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Imagem do Produto")]
    public IFormFile? ImagemUpload { get; set; } 

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a zero")]
    [Display(Name = "Qtde Estoque")]
    public int QuantidadeEstoque { get; set; }

    public bool Ativo { get; set; }

    public string VendedorId { get; set; }
    public virtual Vendedor Vendedor { get; set; }

    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }

    public virtual Categoria Categoria { get; set; }
}
