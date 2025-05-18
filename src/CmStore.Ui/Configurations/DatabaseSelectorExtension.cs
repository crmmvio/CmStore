using CmStore.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CmStore.Ui.Configurations;

public static class DatabaseSelectorExtension
{
    public static void AddDatabaseSelector(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"))
            );
        }
        else
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
        }
    }
}
