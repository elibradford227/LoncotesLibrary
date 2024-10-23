using LoncotesLibrary.models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.AspNetCore.Http;
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

app.MapGet("/api/materials", async (LoncotesLibraryDbContext db, HttpContext httpContext) => 
{
    string materialParam = httpContext.Request.Query["materialType"];
    string genreParam = httpContext.Request.Query["genreParam"];

    var query = db.Materials
        .Where(m => m.OutOfCirculationSince == null)
        .Include(c => c.MaterialType)
        .Include(g => g.Genre)
        .AsQueryable();
    
    if (materialParam != null && int.TryParse(materialParam, out int materialTypeId))
    {
        query = query.Where(m => m.MaterialTypeId == materialTypeId);
    }

    if (genreParam != null && int.TryParse(genreParam, out int genreId))
    {
        query = query.Where(m => m.GenreId == genreId);
    }

    var materials = await query
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
        }).ToListAsync();

    return materials;
});

app.MapGet("/api/materials/{id}", async (LoncotesLibraryDbContext db, int id, HttpContext httpContext) => 
{       
    return db.Materials
        .Include(c => c.MaterialType)
        .Include(g => g.Genre)
        .Include(c => c.Checkouts)
        .ThenInclude(p => p.Patron)
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
                },
                Checkouts = c.Checkouts.Select(ch => new CheckoutDTO
                {
                    Id = ch.Id,
                    PatronId = ch.PatronId,
                    Patron = new PatronDTO 
                    {
                        Id = ch.Patron.Id,
                        FirstName = ch.Patron.FirstName,
                        LastName = ch.Patron.LastName,
                        Address = ch.Patron.Address,
                        Email = ch.Patron.Email,
                        isActive = ch.Patron.isActive
                    },
                    CheckoutDate = ch.CheckoutDate,
                    ReturnDate = ch.ReturnDate
                }).ToList()
            }).Single(c => c.Id == id);
});

app.Run();
