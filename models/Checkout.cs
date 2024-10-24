using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.models;
public class Checkout
{
  public int Id { get; set; }
  [Required]
  public int MaterialId { get; set; }
  [Required]
  public Material Material { get; set; }
  [Required]
  public int PatronId { get; set; }
  [Required]
  public Patron Patron { get; set; }
  [Required]
  public DateTime CheckoutDate { get; set; }
  public DateTime? ReturnDate { get; set; }
}