//-----------------------------------------------------------------------
// <copiright file="UserResponse.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class UserResponse(string userName)
    {
        public string UserName { get; set; } = userName;
    }
}
