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

    public async Task UpdateClient(int id, CreateBaseClientDto dto)
    {
        switch (dto)
        {
            case CreateIndividualClientDto individual:
                var client = await ctx.Clients
                    .OfType<IndividualClient>()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (client is null)
                {
                    throw new NoSuchClientException();
                }
                if (individual.Pesel != client.Pesel)
                {
                    throw new IllegalModificationException("cannot change pesel");
                }
                client.FirstName = individual.FirstName;
                client.LastName = individual.LastName;
                client.Email = individual.Email;
                client.PhoneNumber = individual.Phone;
                break;
            case CreateCompanyClientDto company:
                var clientCompany = await ctx.Clients
                    .OfType<CompanyClient>()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (clientCompany is null)
                {
                    throw new NoSuchClientException();
                }
                if (company.Krs != clientCompany.Krs)
                {
                    throw new IllegalModificationException("cannot change KRS");
                }

                clientCompany.Adres = company.Adres;
                clientCompany.Name = company.Name;
                clientCompany.Email = company.Email;
                clientCompany.PhoneNumber = company.Phone;
                break;
        }

        await ctx.SaveChangesAsync();
    }

    public async Task DeleteClient(int id)
    {
        var client = await ctx.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client is null)
        {
            throw new NoSuchClientException();
        }
        switch (client)
        {
            case IndividualClient individualClient:
                individualClient.FirstName = "";
                individualClient.LastName = "";
                individualClient.Pesel = "";
                individualClient.Email = "";
                individualClient.PhoneNumber = "";
                individualClient.IsDeleted = true;
                break;
            case CompanyClient companyClient:
                throw new IllegalModificationException("company client cannot be deleted");
        }

        await ctx.SaveChangesAsync();
    }
}