using System.ComponentModel.DataAnnotations;
using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }
    public ContractStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public IEnumerable<Discount> ApplicableDiscounts { get; set; } = [];
    public IEnumerable<Payment> Payments { get; set; } = [];
    public Client Client { get; set; } = null!;
    public Product Product { get; set; } = null!;
}