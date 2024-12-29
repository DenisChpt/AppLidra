using AppLidra.Server.Data;
using AppLidra.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppLidra.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JsonDataStore store, IConfiguration configuration) : ControllerBase
{
    private readonly JsonDataStore _store = store;
    private readonly IConfiguration _configuration = configuration;


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel logs)
    {
        User? existingUser = _store.Users.FirstOrDefault(u => u.Email == logs.Email && u.Password == logs.Password);
        if (existingUser == null)
        {
            return Unauthorized("Invalid credentials");
        }

        string token = GenerateJwtToken(existingUser);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_store.Users.Any(u => u.Email == user.Email))
        {
            return BadRequest("User already exists.");
        }
        else if (_store.Users.Any(u => u.UserName == user.UserName))
        {
            return BadRequest("User Name already used.");
        }

        user.Id = _store.Users.Count != 0 ? _store.Users.Max(u => u.Id) + 1 : 1;
        _store.Users.Add(user);
        _store.SaveChanges();

        return Ok();
    }

    [HttpGet("all-users")]
    [AllowAnonymous]
    public IActionResult GetAllUsers()
    {
        return Ok(_store.Users);
    }

    private string GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        string? temp = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not found in configuration.");
        byte[] key = Encoding.UTF8.GetBytes(temp);
        string email = user.Email ?? string.Empty;
        string userId = user.Id.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email)
        ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}