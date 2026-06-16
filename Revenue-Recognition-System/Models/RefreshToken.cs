using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    [MaxLength(8000)]
    public string Token { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public User User { get; set; } = null!;
}