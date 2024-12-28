using AppLidra.Server.Data;
using AppLidra.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppLidra.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(JsonDataStore store) : ControllerBase
{
    private readonly JsonDataStore _store = store;

    [HttpGet("id")]
    public IActionResult GetId()
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(userId);
    }

    [HttpGet("userName")]
    public IActionResult GetUserName()
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        User? user = _store.Users.Where(p => p.Id == userId).First();
        if(user is not null)
            return Ok(new UserResponse(user.UserName));
        else
            return Ok(null);
    }

    [HttpGet("userName/{id}")]
    public IActionResult GetUserName(int id)
    {
        User? user = _store.Users.Where(p => p.Id == id).First();
        if (user is not null)
            return Ok(user.UserName);
        else
            return Ok(null);
    }

    [HttpGet("id/{userName}")]
    public IActionResult GetId(string userName)
    {
        User? user = _store.Users.Where(p => p.UserName == userName).First();
        if (user is not null)
            return Ok(user.Id);
        else
            return Ok(0);
    }

}