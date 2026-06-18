using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class SubscriptionService(DatabaseContext ctx) : ISubsctiptionSercice
{
    public async Task MakeNewSubscription(int clientId, int productId)
    {
        var client = await ctx.Clients.Where(c => c.Id == clientId).FirstOrDefaultAsync();
        if (client is null)
        {
            throw new NoSuchClientException();
        }

        var product = await ctx.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
        if (product is null || product.LicenceType != LicenceType.Subscription)
        {
            throw new NoSuchProductException();
        }

        await ctx.Subscriptions.AddAsync(new Subscription
        {
            Client = client, Payments = new List<SubscriptionPayment>(), Product = product, StartDate = DateTime.Now,
            IsActive = true, RenewalPrice = product.AnnualCost / 12, NextPayment = DateTime.Now.AddDays(30)
        });
        await ctx.SaveChangesAsync();
    }

    public async Task<string> PayForSubscription(int subscriptionId)
    {
        var sub = await ctx.Subscriptions.Where(s => s.Id == subscriptionId)
            .Include(subscription => subscription.Payments).FirstOrDefaultAsync();
        if (sub is null || !sub.IsActive)
        {
            throw new InvalidPaymentException("this subscription does not exist or expired");
        }

        ctx.SubscriptionPayments.AddAsync(new SubscriptionPayment
            { Subscription = sub, Amount = sub.RenewalPrice, PaymentDate = DateTime.Now });
        await ctx.SaveChangesAsync();
        return $"charged {sub.RenewalPrice}PLN";
    }
}