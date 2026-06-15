using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Interfaces;

namespace Revenue_Recognition_System.Models;

[Table("CompanyClients")]
public class CompanyClient : IClient
{
    [Key]
    public int Id { get; set; }
    public ClientTypeId ClientTypeId { get; set; } = ClientTypeId.Company;
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Adres { get; set; } = string.Empty;
    [MaxLength(13)]
    public string Phone { get; set; } = string.Empty;
    [Length(10, 10)]
    public string Krs { get; set; } = string.Empty;
}