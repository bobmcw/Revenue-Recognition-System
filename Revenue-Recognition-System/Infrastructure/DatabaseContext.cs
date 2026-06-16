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
   public DbSet<User> Users { get; set; }
   public DbSet<RefreshToken> RefreshTokens { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Payment>()
         .HasOne(p => p.Client)
         .WithMany(c => c.Payments)
         .HasForeignKey(p => p.ClientId)
         .OnDelete(DeleteBehavior.Restrict);
      modelBuilder.Entity<Payment>()
         .HasOne(p => p.Contract)
         .WithMany(c => c.Payments)
         .HasForeignKey(p => p.ContractId)
         .OnDelete(DeleteBehavior.Restrict);
   }
}
