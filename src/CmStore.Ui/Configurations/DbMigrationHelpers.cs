using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Ui.Configurations;

public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        DbMigrationHelpers.EnsureSeedData(app).Wait();
    }
}

public class DbMigrationHelpers
{
    public static async Task EnsureSeedData(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
        {
            await context.Database.MigrateAsync();

            await EnsureSeedProducts(context);
        }
    }

    private static async Task EnsureSeedProducts(AppDbContext context)
    {
        if (context.Produtos.Any())
            return;

        await context.Categorias.AddRangeAsync(new List<Categoria>()
        {
            new Categoria {Id = 1, Codigo = "Lv001", Nome = "Conhecimentos Gerais", Ativo = true },
            new Categoria {Id = 2, Codigo = "Lv002", Nome = "Informatica", Ativo = true },
            new Categoria {Id = 3, Codigo = "Lv003", Nome = "Economia", Ativo = true }
        });

        await context.SaveChangesAsync();

        var vendedorId = Guid.NewGuid().ToString();

        await context.Users.AddAsync(new IdentityUser
        {
            Id = vendedorId,
            UserName = "teste@crm.com",
            NormalizedUserName = "TESTE@CRM.COM",
            Email = "teste@crm.com",
            NormalizedEmail = "TESTE@CRM.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEI8VDADrqtpXkqh0aUjERlWI1OPHO77GbMmNYMheOGW4PpoSB3HdROpkrVTk9wyefw==",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0
        });

        await context.Vendedores.AddAsync(new Vendedor
        {
            Id = vendedorId,
            Nome = "Cleber Movio",
            Email = "teste@crm.com",
            Ativo = true
        });

        //var roleId = Guid.NewGuid().ToString();
        //await context.Roles.AddAsync(new IdentityRole()
        //{
        //    Id = roleId,
        //    Name = "Admin",
        //    NormalizedName = "ADMIN",
        //    ConcurrencyStamp =  Guid.NewGuid().ToString()
        //});

        //await context.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = vendedorId, RoleId = roleId });

        await context.Produtos.AddRangeAsync(new List<Produto>
        {
            new Produto
            {
                Id = 1,
                Nome = "Pai Rico, Pai Pobre",
                Descricao = "Econômia e Investimento",
                Preco = 100,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv003"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 2,
                Nome = "O Homem mais Rico da Babilônia",
                Descricao = "Econômia e Investimento",
                Preco = 100,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv003"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 3,
                Nome = "O Investidor Inteligente",
                Descricao = "Econômia e Investimento",
                Preco = 100,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv003"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 4,
                Nome = "Fundamentos Html5 e CSS3",
                Descricao = "Desenvolvimento Web",
                Preco = 89,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv002"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 5,
                Nome = "Fundamentos de Java",
                Descricao = "Desenvolvimento Web",
                Preco = 89,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv002"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 6,
                Nome = "Fundamentos de C#",
                Descricao = "Desenvolvimento Web",
                Preco = 89,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv002"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 7,
                Nome = "Fundamentos de C++",
                Descricao = "Desenvolvimento Web",
                Preco = 89,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv002"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
            new Produto
            {
                Id = 8,
                Nome = "Fundamentos de Python",
                Descricao = "Desenvolvimento Web",
                Preco = 89,
                Categoria = context.Categorias.Single(c => c.Codigo == "Lv002"),
                VendedorId = vendedorId,
                Imagem = string.Empty,
                Ativo = true
            },
        });

        await context.SaveChangesAsync();
    }
}
