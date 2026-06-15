using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class ClientService(DatabaseContext ctx) : IClientService
{
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await ctx.Clients.ToListAsync();
    }
}