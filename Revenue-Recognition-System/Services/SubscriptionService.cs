using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public class SubscriptionService(DatabaseContext ctx, IDiscountService discountService) : ISubscriptionService
{
    public async Task<string> MakeNewSubscription(CreateSubscriptionDto dto)
    {
        if (dto.RenewalPeriodMonths is < 1 or > 24)
        {
            throw new InvalidDataException("renewal period must be between 1 and 24 months");
        }

        var client = await ctx.Clients.Where(c => c.Id == dto.ClientId).FirstOrDefaultAsync();
        if (client is null)
        {
            throw new NoSuchClientException();
        }

        var product = await ctx.Products.Where(p => p.Id == dto.ProductId).FirstOrDefaultAsync();
        if (product is null || product.LicenceType != LicenceType.Subscription)
        {
            throw new NoSuchProductException();
        }

        var hasActiveSubscription = await ctx.Subscriptions.AnyAsync(s =>
            s.ClientId == dto.ClientId && s.ProductId == dto.ProductId && s.IsActive);
        var hasActiveContract = await ctx.Contracts.AnyAsync(c =>
            c.Client.Id == dto.ClientId && c.Product.Id == dto.ProductId && c.Status != ContractStatus.Canceled);
        if (hasActiveSubscription || hasActiveContract)
        {
            throw new InvalidDataException("client already has an active contract or subscription for this product");
        }

        var now = DateTime.Now;
        var periodEnd = now.AddMonths(dto.RenewalPeriodMonths);
        var basePrice = product.AnnualCost / 12 * dto.RenewalPeriodMonths;
        var firstPaymentPrice = basePrice;
        var renewalPrice = basePrice;
        List<string> msgs = [];

        var discounts = (await discountService.FindApplicableDiscountsAsync(product)).ToList();
        if (discounts.Count != 0)
        {
            var discount = discounts.OrderByDescending(d => d.DiscountPercentage).First();
            firstPaymentPrice -= firstPaymentPrice * discount.DiscountPercentage / 100;
            msgs.Add($"applied {discount.DiscountPercentage}% {discount.Name} discount;");
        }

        var isLoyalClient =
            await ctx.Contracts.AnyAsync(c => c.Client.Id == dto.ClientId && c.Status == ContractStatus.Paid) ||
            await ctx.Subscriptions.AnyAsync(s => s.ClientId == dto.ClientId && s.Payments.Any());
        if (isLoyalClient)
        {
            firstPaymentPrice -= firstPaymentPrice * 0.05m;
            renewalPrice -= renewalPrice * 0.05m;
            msgs.Add("applied 5% loyal client discount;");
        }

        var subscription = new Subscription
        {
            Client = client,
            Product = product,
            StartDate = now,
            CurrentPeriodStart = now,
            CurrentPeriodEnd = periodEnd,
            RenewalPeriodMonths = dto.RenewalPeriodMonths,
            IsActive = true,
            RenewalPrice = renewalPrice,
            NextPayment = periodEnd,
            Payments = new List<SubscriptionPayment>
            {
                new()
                {
                    Amount = firstPaymentPrice,
                    PaymentDate = now,
                    PeriodStart = now,
                    PeriodEnd = periodEnd
                }
            }
        };

        await ctx.Subscriptions.AddAsync(subscription);
        await ctx.SaveChangesAsync();
        msgs.Add($"charged {firstPaymentPrice}PLN for first subscription period");
        return string.Join(" ", msgs);
    }

    public async Task<string> PayForSubscription(SubscriptionPaymentDto dto)
    {
        var sub = await ctx.Subscriptions.Where(s => s.Id == dto.SubscriptionId)
            .Include(subscription => subscription.Payments).FirstOrDefaultAsync();
        if (sub is null || !sub.IsActive)
        {
            throw new InvalidPaymentException("this subscription does not exist or expired");
        }

        var now = DateTime.Now;
        var periodStart = sub.NextPayment;
        var periodEnd = periodStart.AddMonths(sub.RenewalPeriodMonths);
        if (now < periodStart)
        {
            throw new InvalidPaymentException("current subscription period has already been paid");
        }

        if (now > periodEnd)
        {
            sub.IsActive = false;
            await ctx.SaveChangesAsync();
            throw new ExpiredException("subscription was canceled because previous renewal period was not paid");
        }

        if (dto.Amount != sub.RenewalPrice)
        {
            throw new InvalidPaymentException($"payment amount must be exactly {sub.RenewalPrice}PLN");
        }

        if (sub.Payments.Any(p => p.PeriodStart == periodStart && p.PeriodEnd == periodEnd))
        {
            throw new InvalidPaymentException("this renewal period has already been paid");
        }

        await ctx.SubscriptionPayments.AddAsync(new SubscriptionPayment
        {
            Subscription = sub,
            Amount = dto.Amount,
            PaymentDate = now,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        });
        sub.CurrentPeriodStart = periodStart;
        sub.CurrentPeriodEnd = periodEnd;
        sub.NextPayment = periodEnd;
        await ctx.SaveChangesAsync();
        return $"charged {sub.RenewalPrice}PLN";
    }
}