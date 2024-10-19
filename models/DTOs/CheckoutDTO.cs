namespace LoncotesLibrary.models;
public class CheckoutDTO
{
  public int Id { get; set; }
  public string MaterialId { get; set; }
  public MaterialDTO Material { get; set; }
  public string PatronId { get; set; }
  public PatronDTO Patron { get; set; }
  public DateTime CheckoutDate { get; set; }
  public DateTime ReturnDate { get; set; }
}