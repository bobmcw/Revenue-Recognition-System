using System.ComponentModel.DataAnnotations;
using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public UserRole Role { get; set; }
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Length(64, 64)]
    public string PasswordHash { get; set; } = string.Empty;

    public IEnumerable<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}