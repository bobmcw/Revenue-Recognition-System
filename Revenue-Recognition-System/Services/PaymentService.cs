using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

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
}