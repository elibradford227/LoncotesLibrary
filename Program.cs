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

app.MapPost("/api/materials", (LoncotesLibraryDbContext db, Material material) =>
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

app.MapPut("/api/materials/{id}", (LoncotesLibraryDbContext db, Material material, int id) =>
{
    Material materialToUpdate = db.Materials.SingleOrDefault(m => m.Id == id);
    if (materialToUpdate == null)
    {
        return Results.NotFound();
    }

    materialToUpdate.OutOfCirculationSince = DateTime.Now;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/api/materialTypes", async (LoncotesLibraryDbContext db, HttpContext httpContext) => {
    return db.MaterialTypes
        .Select(c => new MaterialTypeDTO
        {
            Id = c.Id,
            Name = c.Name,
            CheckoutDays = c.CheckoutDays,
        });
});

app.MapGet("/api/genres", async (LoncotesLibraryDbContext db, HttpContext httpContext) => {
    return db.Genres
        .Select(c => new GenreDTO
        {
            Id = c.Id,
            Name = c.Name
        });
});

app.MapGet("/api/patrons", async (LoncotesLibraryDbContext db, HttpContext httpContext) => {
    return db.Patrons
        .Select(ch => new PatronDTO
        {
            Id = ch.Id,
            FirstName = ch.FirstName,
            LastName = ch.LastName,
            Address = ch.Address,
            Email = ch.Email,
            isActive = ch.isActive
        });
});

app.MapPut("/api/patrons/updateEmail/{id}", (LoncotesLibraryDbContext db, Patron patron, int id) =>
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(m => m.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    patronToUpdate.Email = patron.Email;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/api/patrons/updateStatus/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(m => m.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    patronToUpdate.isActive = !patronToUpdate.isActive;

    db.SaveChanges();
    return Results.Ok(patronToUpdate);
});

app.MapPost("/api/checkouts", (LoncotesLibraryDbContext db, Checkout checkout) =>
{
    checkout.CheckoutDate = DateTime.Today;

    db.Checkouts.Add(checkout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{checkout.Id}", checkout);
});

app.MapPut("/api/checkouts/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    Checkout checkoutToUpdate = db.Checkouts.SingleOrDefault(m => m.Id == id);
    if (checkoutToUpdate == null)
    {
        return Results.NotFound();
    }
    checkoutToUpdate.ReturnDate = DateTime.Today;

    db.SaveChanges();
    return Results.Ok(checkoutToUpdate);
});

app.Run();
