//-----------------------------------------------------------------------
// <copiright file="User.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AppLidra.Shared.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
