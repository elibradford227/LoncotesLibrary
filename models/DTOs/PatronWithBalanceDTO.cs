namespace LoncotesLibrary.models;
using System.Collections.Generic;

public class PatronWithBalanceDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public bool isActive { get; set; }

    public PatronWithBalanceDTO(IEnumerable<CheckoutWithLateFeeDTO> checkouts)
    {
        // Calculate FeesOwed during initialization
        FeesOwed = CalculateFeesOwed(checkouts);
    }

    public decimal FeesOwed { get; private set; } = 0;

    private decimal CalculateFeesOwed(IEnumerable<CheckoutWithLateFeeDTO> checkouts)
    {
        return checkouts
            .Where(c => c.LateFee.HasValue) 
            .Sum(c => c.LateFee.Value); 
    }
}