using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.Models;

[Table("Products")]
public class Product
{
   [Key]
   public int Id { get; set; }
   [MaxLength(150)]
   public string Name { get; set; } = string.Empty;
   [MaxLength(500)]
   public string Description { get; set; } = string.Empty;
   [MaxLength(12)]
   public string Version { get; set; } = string.Empty;
   public Category Category { get; set; }
   public LicenceType LicenceType { get; set; }
   [Column(TypeName = "decimal(10,2)")]
   public decimal AnnualCost { get; set; }

   public IEnumerable<Contract> Contracts { get; set; } = [];
}