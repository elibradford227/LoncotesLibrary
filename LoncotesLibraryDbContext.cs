using Microsoft.EntityFrameworkCore;
using LoncotesLibrary.models;

public class LoncotesLibraryDbContext : DbContext
{
  public DbSet<Material> Materials { get; set; }
  public DbSet<Genre> Genres { get; set; }
  public DbSet<Patron> Patrons { get; set; }
  public DbSet<MaterialType> MaterialTypes { get; set; }
  public DbSet<Checkout> Checkouts { get; set; }

  public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
  {

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
      modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
      {
          new MaterialType {Id = 1, Name = "Book", CheckoutDays = 5},
          new MaterialType {Id = 2, Name = "CD", CheckoutDays = 10},
          new MaterialType {Id = 3, Name = "Game", CheckoutDays = 15},
      });
      
      modelBuilder.Entity<Patron>().HasData(new Patron[]
      {
          new Patron {Id = 1, FirstName = "Eli", LastName = "Bradford", Address = "123 Bigboy lane", Email = "bigboy@gmail.com", isActive = true},
          new Patron {Id = 2, FirstName = "Tahlor", LastName = "Tibbs", Address = "456 Bigboy lane", Email = "woofer@gmail.com", isActive = true},
      });

      modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            new Genre { Id = 1, Name = "Science Fiction" },
            new Genre { Id = 2, Name = "Fantasy" },
            new Genre { Id = 3, Name = "Mystery" },
            new Genre { Id = 4, Name = "Romance" },
            new Genre { Id = 5, Name = "Horror" }
        });

      modelBuilder.Entity<Material>().HasData(new Material[]
      {
          new Material { Id = 1, MaterialName = "Dune", MaterialTypeId = 1, GenreId = 1 }, 
          new Material { Id = 2, MaterialName = "The Hobbit", MaterialTypeId = 1, GenreId = 2 }, 
          new Material { Id = 3, MaterialName = "The Da Vinci Code", MaterialTypeId = 1, GenreId = 3, OutOfCirculationSince = new DateTime(2011, 6, 10) }, 
          new Material { Id = 4, MaterialName = "Twilight", MaterialTypeId = 1, GenreId = 4 }, 
          new Material { Id = 5, MaterialName = "IT", MaterialTypeId = 1, GenreId = 5, OutOfCirculationSince = new DateTime(2013, 10, 02) }, 
          
          new Material { Id = 6, MaterialName = "Abbey Road", MaterialTypeId = 2, GenreId = 2 }, 
          new Material { Id = 7, MaterialName = "The Dark Side of the Moon", MaterialTypeId = 2, GenreId = 1 }, 
          
          new Material { Id = 8, MaterialName = "The Witcher 3", MaterialTypeId = 3, GenreId = 2 }, 
          new Material { Id = 9, MaterialName = "L.A. Noire", MaterialTypeId = 3, GenreId = 3 }, 
          new Material { Id = 10, MaterialName = "Resident Evil", MaterialTypeId = 3, GenreId = 5 }
        });
  }
}