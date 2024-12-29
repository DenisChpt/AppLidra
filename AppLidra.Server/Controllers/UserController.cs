using AppLidra.Server.Data;
using AppLidra.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace AppLidra.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(JsonDataStore store) : ControllerBase
    {
        private readonly JsonDataStore _store = store;

        [HttpGet("id")]
        public IActionResult GetId()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            return Ok(userId);
        }

        [HttpGet("userName")]
        public IActionResult GetUserName()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User? user = _store.Users.First(p => p.Id == userId);
            string userName = user.UserName ?? string.Empty;
            return user is not null ? Ok(new UserResponse(userName)) : (IActionResult)Ok(null);
        }

        [HttpGet("userName/{id}")]
        public IActionResult GetUserName(int id)
        {
            User? user = _store.Users.First(p => p.Id == id);
            return user is not null ? Ok(user.UserName) : (IActionResult)Ok(null);
        }

        [HttpGet("id/{userName}")]
        public IActionResult GetId(string userName)
        {
            User? user = _store.Users.First(p => p.UserName == userName);
            return user is not null ? Ok(user.Id) : (IActionResult)Ok(0);
        }
    }
}