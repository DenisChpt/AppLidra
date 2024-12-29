//-----------------------------------------------------------------------
// <copiright file="UserController.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Server.Controllers
{
    using System.Globalization;
    using System.Security.Claims;
    using AppLidra.Server.Data;
    using AppLidra.Shared.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="store">The data store.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(JsonDataStore store) : ControllerBase
    {
        private readonly JsonDataStore _store = store;

        /// <summary>
        /// Gets the user ID of the current user.
        /// </summary>
        /// <returns>The user ID of the current user.</returns>
        [HttpGet("id")]
        public IActionResult GetId()
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            return this.Ok(userId);
        }

        /// <summary>
        /// Gets the user name of the current user.
        /// </summary>
        /// <returns>The user name of the current user.</returns>
        [HttpGet("userName")]
        public IActionResult GetUserName()
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User? user = this._store.Users.First(p => p.Id == userId);
            string userName = user.UserName ?? string.Empty;
            return user is not null ? this.Ok(new UserResponse(userName)) : (IActionResult)this.Ok(null);
        }

        /// <summary>
        /// Gets the user name by user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user name.</returns>
        [HttpGet("userName/{id}")]
        public IActionResult GetUserName(int id)
        {
            User? user = this._store.Users.First(p => p.Id == id);
            return user is not null ? this.Ok(user.UserName) : (IActionResult)this.Ok(null);
        }

        /// <summary>
        /// Gets the user ID by user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The user ID.</returns>
        [HttpGet("id/{userName}")]
        public IActionResult GetId(string userName)
        {
            User? user = this._store.Users.First(p => p.UserName == userName);
            return user is not null ? this.Ok(user.Id) : (IActionResult)this.Ok(0);
        }
    }
}