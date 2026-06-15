using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Interfaces;

namespace Revenue_Recognition_System.Models;

[Table("IndividualClients")]
public class IndividualClient : IClient
{
    [Key]
    public int Id { get; set; }
    public ClientTypeId ClientTypeId { get; set; } = ClientTypeId.Individual;
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;
    [MaxLength(150)]
    public string Adres { get; set; } = string.Empty;

    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Length(11, 11)]
    public string Pesel { get; set; } = string.Empty;
}