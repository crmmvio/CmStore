using CmStore.Domain.Models.Categorias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CmStore.Data.Configuration
{
    public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Codigo)
                   .IsUnicode(false)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(x => x.Nome)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(e => e.Ativo)
                   .IsRequired();

            // 1 : N => Categoria : Produtos
            builder.HasMany(f => f.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
