﻿using System.ComponentModel.DataAnnotations;

namespace CmStore.Core.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public required string Codigo { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public required string Nome { get; set; }

    public bool Ativo { get; set; } = true;

    //public IEnumerable<Produto> Produtos { get; set; }
}
