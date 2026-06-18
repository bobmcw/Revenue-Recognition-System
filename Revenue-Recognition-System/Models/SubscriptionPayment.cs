using System.ComponentModel.DataAnnotations.Schema;

namespace Revenue_Recognition_System.Models;

public class SubscriptionPayment
{
    public int Id { get; set; }


    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
}