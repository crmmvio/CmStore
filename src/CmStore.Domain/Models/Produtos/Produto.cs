using CmStore.Domain.Models.Base;
using CmStore.Domain.Models.Categorias;
using CmStore.Domain.Models.Vendedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmStore.Domain.Models.Produtos
{
    public class Produto : EntityBase
    {

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UnidadeMedida { get; set; } 

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool Ativo { get; set; }

        public Guid VendedorId { get; set; } 
        public Vendedor Vendedor { get; set; }
         
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
