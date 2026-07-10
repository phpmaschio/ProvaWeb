using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ProcessosAPI.Converters;
using ProcessosAPI.Data;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProcessoApiContext>(opts =>
    opts.UseNpgsql(
        builder.Configuration.GetConnectionString("ProcessoConnection"))
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });

builder.Services.AddScoped<StatusProcessoService>();
builder.Services.AddScoped<ParteProcessoService>();
builder.Services.AddScoped<ParteService>();
builder.Services.AddScoped<AndamentoService>();
builder.Services.AddScoped<ProcessoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProcessoApiContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    const int maxRetries = 10;
    for (var tentativa = 1; tentativa <= maxRetries; tentativa++)
    {
        try
        {
            logger.LogInformation("Aplicando migrations (tentativa {Tentativa}/{Max})...", tentativa, maxRetries);
            db.Database.Migrate();
            logger.LogInformation("Migrations aplicadas com sucesso.");
            break;
        }
        catch (Exception ex) when (tentativa < maxRetries)
        {
            logger.LogWarning("Banco ainda não disponível: {Erro}. Tentando novamente em 3s...", ex.Message);
            Thread.Sleep(3000);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (error is NotFoundException notFoundEx)
        {
            logger.LogWarning("{Mensagem}", notFoundEx.Message);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = notFoundEx.Message });
            return;
        }

        logger.LogError(error, "Erro não tratado");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "Erro interno no servidor" });
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();