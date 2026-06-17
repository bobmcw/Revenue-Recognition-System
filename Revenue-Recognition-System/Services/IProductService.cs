using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public interface IProductService
{
   public Task AddNewProductAsync(CreateProductDto dto);
   public Task<string> CreateContractForProductAsync(CreateContractDto dto);

}