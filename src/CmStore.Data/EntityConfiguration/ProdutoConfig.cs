using CmStore.Domain.Models.Produtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CmStore.Data.EntityConfiguration
{
    public class ProdutoConfig : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(x => x.Id).HasName("Pk_Produto");

            builder.Property(e => e.Descricao)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(e => e.QuantidadeEstoque)
                   .IsRequired();

            builder.Property(e => e.UnidadeMedida)
                   .IsUnicode(false)
                   .HasMaxLength(3)            
                   .IsRequired();

            builder.Property(e => e.Preco)
                   .HasPrecision(10,2)
                   .IsRequired();

            builder.Property(e => e.Ativo)
                   .IsRequired();

           
        }
    }
}
