using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.models;
public class Material
{
  public int Id { get; set; }
  [Required]
  public string MaterialName { get; set; }
  [Required]
  public int MaterialTypeId { get; set; }
  public MaterialType MaterialType { get; set; }
  [Required]
  public int GenreId { get; set; }
  public Genre Genre { get; set; }
  public DateTime? OutOfCirculationSince { get; set; }

  public virtual ICollection<Checkout> Checkouts { get; set; }
}