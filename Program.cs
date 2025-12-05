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

    // --- CORRECCIÓN DEFINITIVA DE CONEXIÓN ---
    // 1. Buscamos la variable simple que acabamos de crear en Render
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

    // 2. Si no existe (por ejemplo, en tu PC local), usamos la del appsettings.json
    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    // Debug: Imprimimos esto para ver en los logs si funcionó
    Console.WriteLine($"[DEBUG] Buscando 'DB_CONNECTION'...");
    Console.WriteLine($"[DEBUG] Resultado: {(string.IsNullOrEmpty(connectionString) ? "VACIO/NULL" : "ENCONTRADO (Oculto por seguridad)")}");

    // 3. Conectamos
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
    // -----------------------------------------

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Servicios de la aplicación
    builder.Services.AddScoped<GlobalFunctions>();

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

    // Migraciones automáticas
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    // Swagger siempre visible (para probar en Render)
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseHttpsRedirection();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();