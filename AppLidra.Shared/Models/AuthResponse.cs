//-----------------------------------------------------------------------
// <copiright file="AuthResponse.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents the response received after an authentication request.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string? Token { get; set; }
    }
}
