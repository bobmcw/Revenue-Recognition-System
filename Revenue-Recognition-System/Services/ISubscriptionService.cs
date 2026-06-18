using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public interface ISubscriptionService
{
   public Task<string> MakeNewSubscription(CreateSubscriptionDto dto);
   public Task<string> PayForSubscription(SubscriptionPaymentDto dto);
}
