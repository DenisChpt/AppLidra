//-----------------------------------------------------------------------
// <copiright file="AuthController.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Server.Controllers
{
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using AppLidra.Server.Data;
    using AppLidra.Shared.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="store">The data store.</param>
    /// <param name="configuration">The configuration settings.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(JsonDataStore store, IConfiguration configuration) : ControllerBase
    {
        private readonly JsonDataStore _store = store;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="logs">The login model containing user credentials.</param>
        /// <returns>An IActionResult containing the JWT token if successful, otherwise Unauthorized.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel logs)
        {
            User? existingUser = this._store.Users.FirstOrDefault(u => u.Email == logs.Email && u.Password == logs.Password);
            if (existingUser == null)
            {
                return Unauthorized("Invalid credentials");
            }

            string token = GenerateJwtToken(existingUser);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Registers a new user with the provided user details.
        /// </summary>
        /// <param name="user">The user model containing user details.</param>
        /// <returns>An IActionResult indicating the result of the registration.</returns>
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

            if (user is null)
            {
                return BadRequest("User is null.");
            }

            user.Id = this._store.Users.Count != 0 ? this._store.Users.Max(u => u.Id) + 1 : 1;
            this._store.Users.Add(user);
            this._store.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>An IActionResult containing the list of all users.</returns>
        [HttpGet("all-users")]
        [AllowAnonymous]
        public IActionResult GetAllUsers()
        {
            return Ok(_store.Users);
        }

        private string GenerateJwtToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new ();
            string? temp = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not found in configuration.");
            byte[] key = Encoding.UTF8.GetBytes(temp);
            string email = user.Email ?? string.Empty;
            string userId = user.Id.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
            SecurityTokenDescriptor tokenDescriptor = new ()
            {
                Subject = new ClaimsIdentity(
                [
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            ]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}