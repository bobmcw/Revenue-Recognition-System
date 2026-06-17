using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public interface IClientService
{
    public Task<IEnumerable<Client>> GetAllAsync();
    public Task PostClient(CreateBaseClientDto dto);
    public Task UpdateClient(int id, CreateBaseClientDto dto);
    public Task DeleteClient(int id);
}