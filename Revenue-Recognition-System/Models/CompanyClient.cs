using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revenue_Recognition_System.Models;

[Table("CompanyClients")]
public class CompanyClient : Client
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Adres { get; set; } = string.Empty;
    [Length(10, 10)]
    public string Krs { get; set; } = string.Empty;
}