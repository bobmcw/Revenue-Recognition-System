using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.PostDTOs;

namespace Revenue_Recognition_System.Services;

public class ClientService(DatabaseContext ctx) : IClientService
{
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await ctx.Clients.ToListAsync();
    }

    public async Task PostClient(CreateBaseClientDto dto)
    {
        Client newClient;
        switch (dto)
        {
            case CreateIndividualClientDto individial:
                newClient = new IndividualClient
                {
                    FirstName = individial.FirstName,
                    LastName = individial.LastName,
                    Pesel = individial.Pesel,
                    Email = individial.Email,
                    PhoneNumber = individial.Phone
                };
                break;
            case CreateCompanyClientDto company:
                newClient = new CompanyClient
                {
                    Name = company.Name,
                    Adres = company.Adres,
                    Krs = company.Krs,
                    Email = company.Email,
                    PhoneNumber = company.Phone
                };
                break;
            default:
                throw new UnknownClientTypeException();
        }

        await ctx.Clients.AddAsync(newClient);
        await ctx.SaveChangesAsync();
    }
}