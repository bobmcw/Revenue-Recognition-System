using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IUserService
{
    public Task CreateUserAsync(string username, string password);
    public Task<User> AuthenticateAsync(string username, string password);
    public Task SaveRefreshTokenAsync(int id, string token);
    public Task<User?> FindByRefreshTokenAsync(string token);
    public Task<RefreshToken?> GetRefreshTokenAsync(User u, string token);
}