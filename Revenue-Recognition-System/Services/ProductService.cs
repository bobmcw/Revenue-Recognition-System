using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public class ProductService(DatabaseContext ctx, IDiscountService discountService) : IProductService
{
    public async Task AddNewProductAsync(CreateProductDto dto)
    {
        var prod = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            AnnualCost = dto.AnnualCost,
            Category = dto.Category,
            LicenceType = dto.LicenceType,
            Version = dto.Version
        };
        await ctx.Products.AddAsync(prod);
        await ctx.SaveChangesAsync();
    }

    public async Task<string> CreateContractForProductAsync(CreateContractDto dto)
    {
        var client = await ctx.Clients.Include(client => client.Contracts).ThenInclude(contract => contract.Product).FirstOrDefaultAsync(c => c.Id == dto.ClientId);
        if (client is null)
        {
            throw new NoSuchClientException();
        }

        var prod = await ctx.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
        if (prod is null)
        {
            throw new NoSuchProductException();
        }

        if (client.Contracts.Any(c => c.Product == prod))
        {
            throw new InvalidDataException("client already has a contract for this product");
        }

        List<string> msgs = [];
        var discounts = (await discountService.FindApplicableDiscountsAsync(prod)).ToList();
        var cost = prod.AnnualCost;
        
        //additional support cost
        if (dto.AdditionalYears is < 0 or > 3)
        {
            throw new InvalidDataException("aditional years must be between 1 and 3");
        }
        cost += 1000 * dto.AdditionalYears;
        if (dto.AdditionalYears > 0)
        {
            msgs.Add($"billed {1000 * dto.AdditionalYears}PLN for {dto.AdditionalYears} years of aditional support;");
        }
        
        //returning customer discount
        if ((await ctx.Contracts.Where(c => c.Client == client && c.Status == ContractStatus.Paid).ToListAsync()).Count > 0)
        {
            cost -= cost * 0.05m;
            msgs.Add("applied 5% returning client discount;");
        }

        if (discounts.Count != 0)
        {
            var discount = discounts.OrderByDescending(d => d.DiscountPercentage).First();
            cost -= 1.0m / discount.DiscountPercentage;
            msgs.Add($"applied {discount.DiscountPercentage}% {discount.Name} discount;");
        }
        var contract = new Contract
        {
            Client = client,
            Product = prod,
            StartDate = DateTime.Now,
            EndDate = dto.EndDate ?? DateTime.Now.AddDays(14),
            Cost = cost,
            Payments = new List<Payment>(),
            Status = ContractStatus.Created,
            ApplicableDiscounts = discounts
        };
        await ctx.Contracts.AddAsync(contract);
        await ctx.SaveChangesAsync();
        return string.Join(" ", msgs);
    }
}