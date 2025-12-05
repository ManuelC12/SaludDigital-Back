using Microsoft.EntityFrameworkCore;
using SaludDigital.Helpers;
using SaludDigital.Data;
using MediatR;
using FluentValidation;
using System.Reflection;
using SaludDigital.Aplication.User;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

// --- CAMBIO 1: BÚSQUEDA ROBUSTA DE LA CONEXIÓN ---
// Primero intentamos leer la configuración normal
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Si está vacía (que es lo que te está pasando), la buscamos manualmente en las variables de entorno de Render
if (string.IsNullOrEmpty(connectionString))
{
    // Buscamos la variable exacta que pusimos en Render
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
}

// Imprimimos en consola para depurar (esto saldrá en los logs de Render si falla)
Console.WriteLine($"Cadena de conexión encontrada: {(string.IsNullOrEmpty(connectionString) ? "NO" : "SI")}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
// -------------------------------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- TUS SERVICIOS ---
builder.Services.AddScoped<GlobalFunctions>();

// MediatR y validadores
builder.Services.AddMediatR(typeof(RegisterUser.Manejador).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUser.Validador).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Migraciones automáticas al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Esto asegura que la base de datos esté sincronizada
    db.Database.Migrate();
}

// --- CAMBIO 2: SWAGGER SIEMPRE VISIBLE ---
// Quitamos el "if (IsDevelopment)" para que puedas probar tu API en Render
app.UseSwagger();
app.UseSwaggerUI();
// -----------------------------------------

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();