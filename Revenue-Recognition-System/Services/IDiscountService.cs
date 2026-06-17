using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IDiscountService
{
   public Task<IEnumerable<Discount>> FindApplicableDiscountsAsync(Product p);
}