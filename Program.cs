using LoncotesLibrary.models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDBConnectionString"]);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/materials", (LoncotesLibraryDbContext db) => 
{
    return db.Materials
    .Where(m => m.OutOfCirculationSince == null)
    .Include(c => c.MaterialType)
    .Include(g => g.Genre)
    .Select(c => new MaterialDTO
    {
        Id = c.Id,
        MaterialName = c.MaterialName,
        MaterialTypeId = c.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = c.MaterialType.Id,
            Name = c.MaterialType.Name,
            CheckoutDays = c.MaterialType.CheckoutDays,
        },
        GenreId = c.GenreId,
        Genre = new GenreDTO
        {
            Id = c.Genre.Id,
            Name = c.Genre.Name
        }
    }).ToList();
});

app.Run();
