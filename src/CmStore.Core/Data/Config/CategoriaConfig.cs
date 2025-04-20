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


            builder.HasIndex(x => x.Codigo)
                   .IsUnique()
                   .HasDatabaseName("IX_Categorias_Codigo");

            // 1 : N => Categoria : Produtos
            builder.HasMany(f => f.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
