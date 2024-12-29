//-----------------------------------------------------------------------
// <copiright file="JsonData.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
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

