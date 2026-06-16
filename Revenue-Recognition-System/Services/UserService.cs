using System.Security.Cryptography;
using System.Text;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class UserService(DatabaseContext ctx) : IUserService
{
    private bool _isPasswordValid(string password)
    {
        return password.Any(char.IsUpper) && password.Length > 6 && password.Length < 50;
    }
    public async Task CreateUserAsync(string username, string password)
    {
        if (!_isPasswordValid(password))
        {
            throw new PasswordPolicyException();
        }

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        await ctx.Users.AddAsync(new User { Username = username, PasswordHash = hash, Role = UserRole.Regular });

    }

    public Task<User?> AuthenticateAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task SaveRefreshTokenAsync(int id, string token)
    {
        throw new NotImplementedException();
    }
}