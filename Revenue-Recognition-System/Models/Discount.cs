using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revenue_Recognition_System.Models;

[Table("Discounts")]
public class Discount
{
   [Key]
   public int Id { get; set; }
   [MaxLength(100)]
   public string Name { get; set; } = string.Empty;
   [Column(TypeName = "decimal(10,2)")]
   public decimal DiscountPercentage { get; set; }
   public DateTime StartDate { get; set; } 
   public DateTime EndDate { get; set; }

   public IEnumerable<Product> Products { get; set; } = [];
}