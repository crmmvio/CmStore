using CmStore.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace CmStore.Core.Data;

public class AppDbContext : IdentityDbContext
{
    public bool UsingSqlLite { get; private set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        UsingSqlLite = !string.IsNullOrWhiteSpace(configuration.GetConnectionString("DefaultConnectionLite"));
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Vendedor> Vendedores { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        //foreach (var property in builder.Model.GetEntityTypes()
        //                                      .SelectMany(e => e.GetProperties()
        //                                      .Where(p => p.ClrType == typeof(string))))
        //{
        //    var maxLength = property.GetMaxLength();
        //    property.SetColumnType($"varchar({maxLength.GetValueOrDefault(100)})");
        //}

        base.OnModelCreating(builder);
    }
}
