using CmStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmStore.Core.Data.Config
{
    public class ProdutoConfig : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(x => x.Id).HasName("Pk_Produto");

            builder.Property(e => e.Nome)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(e => e.Descricao)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(e => e.Imagem)
                   .IsUnicode(false)
                   .HasMaxLength(1000);

            builder.Property(e => e.Preco)
                   .HasPrecision(10, 2)
                   .IsRequired();

            builder.Property(e => e.QuantidadeEstoque)
                   .IsRequired();

            builder.Property(e => e.Ativo)
                   .IsRequired();
        }
    }
}
