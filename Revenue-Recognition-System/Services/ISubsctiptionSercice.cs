namespace Revenue_Recognition_System.Services;

public interface ISubsctiptionSercice
{
   public Task MakeNewSubscription(int clientId, int productId);
   public Task<string> PayForSubscription(int subscriptionId);
}