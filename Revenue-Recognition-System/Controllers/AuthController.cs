using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserService _users;

    public AuthController(IConfiguration config, IUserService users)
    {
        _config = config;
        _users = users;
    }
    public record LoginDto(string Username, string Password);
    public record TokensDto(string AccessToken, string RefreshToken);

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _users.AuthenticateAsync(dto.Username, dto.Password);
        if (user is null)
        {
            return Unauthorized();
        }

        var tokens = GenerateTokens(user);
        await _users.SaveRefreshTokenAsync(user.Id, tokens.RefreshToken);

        return Ok(tokens);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginDto dto)
    {
        try
        {
            await _users.CreateUserAsync(dto.Username, dto.Password);
        }
        catch (PasswordPolicyException e)
        {
            return BadRequest(e.Message);
        }

        return Created();
    }
    private TokensDto GenerateTokens(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refreshToken = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));

        return new TokensDto(accessToken, refreshToken);
    }
}