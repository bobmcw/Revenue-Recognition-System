using System.ComponentModel.DataAnnotations.Schema;

namespace Revenue_Recognition_System.Models;

public class Payment
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(10,2)")] 
    public decimal Amount { get; set; }

    public Client Client { get; set; } = null!;
    public Contract Contract { get; set; } = null!;
}