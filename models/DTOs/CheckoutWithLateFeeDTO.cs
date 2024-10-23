using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LoncotesLibrary.models;
public class CheckoutWithLateFeeDTO
{
  public int Id { get; set; }
  public int MaterialId { get; set; }
  public MaterialDTO Material { get; set; }
  public int PatronId { get; set; }
  public PatronWithBalanceDTO Patron { get; set; }
  public DateTime CheckoutDate { get; set; }
  public DateTime? ReturnDate { get; set; }

  private static decimal _lateFeePerDay = .50m;

  public decimal? LateFee
  {
      get
      {
        DateTime dueDate  = CheckoutDate.AddDays(Material.MaterialType.CheckoutDays);
        DateTime returnDate = ReturnDate ?? DateTime.Today;
        int daysLate = (returnDate - dueDate).Days;
        decimal fee = daysLate * _lateFeePerDay;
        return daysLate > 0 ? fee : null;
      }
  }

  // Private paid field, set to false by default

  private bool Paid { get; set; } = false;

  //  Constructor to return if LateFee was paid if it exists.
  //  The ternary returns Paid (false) if LateFee is greater than 0, and returns true if otherwise
  public bool isPaid
  {
    get
    {
      return LateFee > 0 ? Paid : true;
    }
  }
}