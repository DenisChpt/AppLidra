//-----------------------------------------------------------------------
// <copiright file="ProjectRenameModel.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a model for renaming a project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="newName">The new name of the project.</param>
    public class ProjectRenameModel(int projectId, string newName)
    {
        /// <summary>
        /// Gets or sets the project ID.
        /// </summary>
        public int ProjectId { get; set; } = projectId;

        /// <summary>
        /// Gets or sets the new name of the project.
        /// </summary>
        public string NewName { get; set; } = newName;
    }
}
