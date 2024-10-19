using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.models;
public class Genre 
{
  public int Id { get; set; }
  [Required]
  public string Name { get; set; }
}