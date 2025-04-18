using CmStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace CmStore.Api.Configurations
{
    public static class DatabaseContextConfig
    {
        public static WebApplicationBuilder AddDatabaseContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }
    }
}
