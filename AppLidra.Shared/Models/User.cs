//-----------------------------------------------------------------------
// <copiright file="User.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a user in the application.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Required]
        [MinLength(4)]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
