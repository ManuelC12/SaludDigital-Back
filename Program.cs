using Microsoft.EntityFrameworkCore;
using SaludDigital.Helpers;
using SaludDigital.Data;
using MediatR;
using FluentValidation;
using System.Reflection;
using SaludDigital.Aplication.User;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GlobalFunctions>();

builder.Services.AddMediatR(typeof(RegisterUser.Manejador).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUser.Validador).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
