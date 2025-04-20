using CmStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CmStore.Core.Data.Config
{
    public class VendedorConfig : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedores");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .IsUnicode(false)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(e => e.Ativo)
                   .IsRequired();

            // 1 : N => Vendedor : Produtos
            builder.HasMany(f => f.Produtos)
                .WithOne(p => p.Vendedor)
                .HasForeignKey(p => p.VendedorId);
        }
    }
}
