namespace Revenue_Recognition_System.PostDTOs;

public class CreateSubscriptionDto
{
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int RenewalPeriodMonths { get; set; } = 1;
}
