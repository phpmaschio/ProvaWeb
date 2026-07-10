using Microsoft.EntityFrameworkCore;
using ProcessosAPI.Converters;
using ProcessosAPI.Data;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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