using Microsoft.EntityFrameworkCore;
using BonusSystem.Api.Data;
using BonusSystem.Api.Services;
using BonusSystem.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. חיבור SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=bonus_system.db"));

// 2. רישום שירותים
builder.Services.AddScoped<IIngestionService, IngestionService>();

// 3. הגדרת CORS עבור Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();