using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

    private string _calculateSha256Hash(string s)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s)));
    }
    public async Task CreateUserAsync(string username, string password)
    {
        if (!_isPasswordValid(password))
        {
            throw new PasswordPolicyException();
        }

        var hash = _calculateSha256Hash(password);
        await ctx.Users.AddAsync(new User { Username = username, PasswordHash = hash, Role = UserRole.Regular });

        await ctx.SaveChangesAsync();
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var usr = await ctx.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (usr is null)
        {
            throw new NoSuchUserException();
        }

        if (usr.PasswordHash != _calculateSha256Hash(password))
        {
            throw new InvalidPasswordException();
        }

        return usr;

    }

    public async Task SaveRefreshTokenAsync(int id, string token)
    {
        var usr = ctx.Users.FirstOrDefault(u => u.Id == id);
        if (usr is null)
        {
            throw new NoSuchUserException();
        }

        await ctx.RefreshTokens.AddAsync(new RefreshToken
            { CreationDate = DateTime.Now, ExpiryDate = DateTime.Now.AddMinutes(20), Token = token, User = usr });
        await ctx.SaveChangesAsync();
    }

    public async Task<User?> FindByRefreshTokenAsync(string token)
    {
        return await ctx.Users.Where(u => u.RefreshTokens.Any(t => t.Token == token)).FirstOrDefaultAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(User u, string token)
    {
        return await ctx.RefreshTokens.Where(t => t.User.Id == u.Id && t.Token == token).FirstOrDefaultAsync();
    }
}