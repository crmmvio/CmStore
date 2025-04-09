using CmStore.Data.Contexts;
using Microsoft.EntityFrameworkCore;

ILogger<Program> logger = null;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Add Logger
    var loggerFactory = LoggerFactory.Create(logging =>
    {
        logging.Configure(options =>
        {
            options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                            | ActivityTrackingOptions.TraceId
                                            | ActivityTrackingOptions.ParentId;
        }).AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
        });
    });

    logger = loggerFactory.CreateLogger<Program>();
    logger.LogInformation(">>>> Enviroment: {EnvironmentName} <<<<", builder.Environment.EnvironmentName);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (TaskCanceledException ex)
{
    var message = $"Cancelamento de Task foi detectado, saindo do programa ={ex.Message}";
    logger?.LogError(ex, message);
}
catch (Exception ex)
{
    var message = $"Erro de inicialização ou finalização doprograma = {ex.Message}\r\nStackTrace = {ex.StackTrace}";
    logger?.LogError(ex, message);
}
