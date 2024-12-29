//-----------------------------------------------------------------------
// <copiright file="Project.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "New Project";
        public DateTime CreatedAt { get; set; }
        public int OwnerUserId { get; set; }
        public List<string> Collaborators { get; set; } = [];
    }
}
