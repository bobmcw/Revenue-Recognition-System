using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IClientService
{
    public Task<IEnumerable<Client>> GetAllAsync();
}