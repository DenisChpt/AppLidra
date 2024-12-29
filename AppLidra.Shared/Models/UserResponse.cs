//-----------------------------------------------------------------------
// <copiright file="UserResponse.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class UserResponse(string userName)
    {
        public string UserName { get; set; } = userName;
    }
}
