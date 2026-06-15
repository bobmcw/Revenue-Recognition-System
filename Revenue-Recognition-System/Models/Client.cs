using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.Models;

[Table("Clients")]
public abstract class Client
{
   [Key]
   public int Id { get; set; }
   // public ClientType ClientType { get; set; }
   
   //contact info
   [MaxLength(50)]
   public string Email { get; set; } = string.Empty;
   [MaxLength(15)]
   public string PhoneNumber { get; set; } = string.Empty;

   public IEnumerable<Contract> Contracts { get; set; } = [];
}