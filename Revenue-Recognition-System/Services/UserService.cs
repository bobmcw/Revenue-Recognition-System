using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Infrastructure;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class UserService(DatabaseContext ctx) : IUserService
{
    public Task<User?> AuthenticateAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task SaveRefreshTokenAsync(int id, string token)
    {
        throw new NotImplementedException();
    }
}