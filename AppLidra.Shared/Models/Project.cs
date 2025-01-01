//-----------------------------------------------------------------------
// <copiright file="Project.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a project with its details.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the ID of the project.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; } = "New Project";

        /// <summary>
        /// Gets or sets the date and time when the project was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the owner user.
        /// </summary>
        public int OwnerUserId { get; set; }

        /// <summary>
        /// Gets or sets the list of collaborators.
        /// </summary>
        public List<string> Collaborators { get; set; } = [];
    }
}
