//-----------------------------------------------------------------------
// <copiright file="JsonData.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

using AppLidra.Shared.Models;

namespace AppLidra.Shared.Services
{
    public class JsonData
    {
        public List<Project> Projects { get; set; } = [];
        public List<User> Users { get; set; } = [];
        public List<Expense> Expenses { get; set; } = [];
    }
}

