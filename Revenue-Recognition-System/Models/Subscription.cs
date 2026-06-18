namespace Revenue_Recognition_System.Models;

public class Subscription
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public decimal RenewalPrice { get; set; }
    
    public DateTime NextPayment { get; set; }
    
    public bool IsActive { get; set; }

    public List<SubscriptionPayment> Payments { get; set; } = new List<SubscriptionPayment>();
}