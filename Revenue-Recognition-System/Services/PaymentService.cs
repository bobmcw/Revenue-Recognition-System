using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public class PaymentService(DatabaseContext ctx) : IPaymentService
{
    public async Task<string> MakePaymentAsync(int contractId, decimal amount)
    {
        var contract = ctx.Contracts.Include(contract => contract.Client).FirstOrDefault(c => c.Id == contractId);
        if (contract is null)
        {
            throw new NoSuchContractException();
        }
        if (contract.Status != ContractStatus.Created)
        {
            throw new InvalidPaymentException("cannot pay for a paid or canceled contract");
        }

        if (contract.EndDate < DateTime.Now)
        {
            contract.Status = ContractStatus.Canceled;
            await ctx.SaveChangesAsync();
            throw new ExpiredException("contract expired and has beed canceled");
        }

        var payments = await ctx.Payments.Where(p => p.ContractId == contract.Id).ToListAsync();
        decimal sum = 0.0m;
        foreach (var payment in payments)
        {
            sum += payment.Amount;
        }

        List<string> msgs = new List<string>();
        decimal toPay;
        if (sum + amount > contract.Cost)
        {
            toPay = contract.Cost - sum;
            msgs.Add($"tried to pay {amount}PLN but only {toPay}PLN is left to pay; Accepting {toPay}PLN");
        }
        else
        {
            toPay = amount;
        }

        if (toPay + sum == contract.Cost)
        {
            contract.Status = ContractStatus.Paid;
            msgs.Add("contract was fully paid!");
        }
        await ctx.Payments.AddAsync(new Payment { Amount = toPay, Client = contract.Client, Contract = contract });
        await ctx.SaveChangesAsync();
        return string.Join(" ", msgs);
    }

    private async Task<decimal> GetExchangeRateAsync(string currency)
    {
        if (currency.Equals("PLN", StringComparison.OrdinalIgnoreCase))
        {
            return 1m;
        }
        currency = currency.ToLower();
        using var httpClient = new HttpClient();
        var url = $"https://api.nbp.pl/api/exchangerates/rates/a/{currency}/?format=json";
        var json = await httpClient.GetStringAsync(url);
        using var document = JsonDocument.Parse(json);
        try
        {
            var rate = document
                .RootElement
                .GetProperty("rates")[0]
                .GetProperty("mid")
                .GetDecimal();
            return rate;
        }
        catch (KeyNotFoundException)
        {
            throw new InvalidCurrencyException();
        } 
    }
    
    public async Task<RevenueDto> CalculateRevenueAsync(string? currency)
    {
        decimal revenue = 0.0m;
        var paidContracts = await ctx.Contracts.Where(c => c.Status == ContractStatus.Paid).ToListAsync();
        foreach (var paidContract in paidContracts)
        {
            revenue += paidContract.Cost;
        }
        revenue += await ctx.SubscriptionPayments.SumAsync(p => p.Amount);

        decimal expectedRecenue = 0.0m;
        var pendingContracts = await ctx.Contracts.Where(c => c.Status == ContractStatus.Created).ToListAsync();
        foreach (var pendingContract in pendingContracts)
        {
            expectedRecenue += pendingContract.Cost;
        }
        expectedRecenue += await ctx.Subscriptions.Where(s => s.IsActive).SumAsync(s => s.RenewalPrice);

        expectedRecenue += revenue;
        if (currency != null)
        {
            var rate = await GetExchangeRateAsync(currency);
            return new RevenueDto { Revenue = revenue / rate , ExpectedRevenue = expectedRecenue / rate };
        }
        return new RevenueDto { Revenue = revenue, ExpectedRevenue = expectedRecenue };
    }

    public async Task<RevenueDto> CalculateRevenueAsync(int productId, string? currency)
    {
        decimal revenue = 0.0m;
        var paidContracts = await ctx.Contracts.Where(c => c.Status == ContractStatus.Paid && c.Product.Id == productId).ToListAsync();
        foreach (var paidContract in paidContracts)
        {
            revenue += paidContract.Cost;
        }
        revenue += await ctx.SubscriptionPayments
            .Where(p => p.Subscription.ProductId == productId)
            .SumAsync(p => p.Amount);

        decimal expectedRecenue = 0.0m;
        var pendingContracts = await ctx.Contracts.Where(c => c.Status == ContractStatus.Created && c.Product.Id == productId).ToListAsync();
        foreach (var pendingContract in pendingContracts)
        {
            expectedRecenue += pendingContract.Cost;
        }
        expectedRecenue += await ctx.Subscriptions
            .Where(s => s.IsActive && s.ProductId == productId)
            .SumAsync(s => s.RenewalPrice);

        expectedRecenue += revenue;
        if (currency != null)
        {
            var rate = await GetExchangeRateAsync(currency);
            return new RevenueDto { Revenue = revenue / rate, ExpectedRevenue = expectedRecenue / rate };
        }
        return new RevenueDto { Revenue = revenue, ExpectedRevenue = expectedRecenue };
    }
}