using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Infrastructure;

public class DatabaseContext(DbContextOptions opt) : DbContext(opt)
{
   public DbSet<Client> Clients { get; set; }
   public DbSet<CompanyClient> CompanyClients { get; set; }
   public DbSet<IndividualClient> IndividualClients { get; set; }
   public DbSet<Contract> Contracts { get; set; }
   public DbSet<Discount> Discounts { get; set; }
   public DbSet<Product> Products { get; set; }
}
