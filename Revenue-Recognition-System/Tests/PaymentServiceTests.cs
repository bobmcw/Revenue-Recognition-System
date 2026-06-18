using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Services;
using Xunit;

namespace Revenue_Recognition_System.Tests;

public class PaymentServiceTests
{
    private readonly DatabaseContext _context;
    private readonly PaymentService _sut; // System Under Test
    public PaymentServiceTests()
    {

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            // Używaj unikalnej nazwy bazy dla każdego testu (np. Guid), 
            // aby testy nie nadpisywały sobie nawzajem danych!
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        Client c = new CompanyClient { Id = 1 };
        _context.Clients.Add(c);
        _context.Contracts.Add(new Contract { Id = 1, Cost = 1000.0m, Client = c});
        _context.Contracts.Add(new Contract { Id = 2, Cost = 1000.0m, Client = c, Status = ContractStatus.Canceled});
        _context.SaveChanges();
        _sut = new PaymentService(_context);
    }

    [Fact]
    public async Task MakePayment_DoesntPayOverCost()
    {
        await _sut.MakePaymentAsync(1, 500.0m);
        await _sut.MakePaymentAsync(1, 550.0m);
        
        Assert.Equal(2, _context.Payments.Count());
        var list = await _context.Payments.ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(500.0m, list[0].Amount);
        Assert.Equal(500.0m, list[1].Amount);
    }

    [Fact]
    public async Task MakePayment_ThrowsWhen_ContractIsCanceled()
    {
        await Assert.ThrowsAsync<InvalidPaymentException>(() => _sut.MakePaymentAsync(2, 1.0m));
    }

    [Fact]
    public async Task MakePayment_ThrowsWhen_PayingForContractThatDoesntExist()
    {
        await Assert.ThrowsAnyAsync<NoSuchContractException>(() => _sut.MakePaymentAsync(5, 10.0m));
    }
}