using System.ComponentModel.DataAnnotations.Schema;

namespace Revenue_Recognition_System.Models;

public class Payment
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(10,2)")] 
    public decimal Amount { get; set; }

    public int ClientId { get; set; }
    public int ContractId { get; set; }
    [ForeignKey(nameof(ClientId))] public Client Client { get; set; } = null!;
    [ForeignKey(nameof(ContractId))] public Contract Contract { get; set; } = null!;
}