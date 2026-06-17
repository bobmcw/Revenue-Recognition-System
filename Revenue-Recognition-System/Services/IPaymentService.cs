namespace Revenue_Recognition_System.Services;

public interface IPaymentService
{
    public Task<string> MakePaymentAsync(int contractId, decimal amount);
}