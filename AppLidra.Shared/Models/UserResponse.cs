//-----------------------------------------------------------------------
// <copiright file="UserResponse.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a response from a user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserResponse"/> class.
    /// </remarks>
    /// <param name="username">The user name.</param>
    public class UserResponse(string userName)
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName { get; set; } = userName;
    }
}