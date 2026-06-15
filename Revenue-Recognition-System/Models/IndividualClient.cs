using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Revenue_Recognition_System.BaseClasses;

namespace Revenue_Recognition_System.Models;

[Table("IndividualClients")]
public class IndividualClient : Client
{
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Length(11, 11)]
    public string Pesel { get; set; } = string.Empty;
}