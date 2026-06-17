using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public interface IPaymentService
{
    public Task<string> MakePaymentAsync(int contractId, decimal amount);
    public Task<RevenueDto> CalculateRevenueAsync();
    public Task<RevenueDto> CalculateRevenueAsync(int productId);
}