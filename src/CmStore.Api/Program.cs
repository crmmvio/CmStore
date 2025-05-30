using CmStore.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseSelector();
builder.AddApiConfig()
       .AddCorsConfig()
       .AddSwaggerConfig()
       .AddDatabaseContextConfig()
       .AddIdentityConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();
