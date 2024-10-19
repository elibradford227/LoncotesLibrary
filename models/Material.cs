using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.models;
public class Material
{
  public int Id { get; set; }
  [Required]
  public string MaterialName { get; set; }
  [Required]
  public string MaterialTypeId { get; set; }
  public MaterialType MaterialType { get; set; }
  [Required]
  public string GenreId { get; set; }
  public Genre Genre { get; set; }
  public DateTime OutOfCirculationSince { get; set; }
}