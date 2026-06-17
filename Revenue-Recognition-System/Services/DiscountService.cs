using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class DiscountService(DatabaseContext ctx) : IDiscountService
{
    public async Task<IEnumerable<Discount>> FindApplicableDiscountsAsync(Product p)
    {
        return await ctx.Discounts
            .Where(d => d.Products.Contains(p) && d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
            .ToListAsync();
    }
    
}