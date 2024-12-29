//-----------------------------------------------------------------------
// <copiright file="CollaboratorModificationModel.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a model for modifying a collaborator by name.
    /// </summary>
    public class CollaboratorModificationModel(int projectId, string collaboratorName)
    {
        /// <summary>
        /// Gets or sets the project ID.
        /// </summary>
        public int ProjectId { get; set; } = projectId;

        /// <summary>
        /// Gets or sets the name of the collaborator.
        /// </summary>
        public string CollaboratorName { get; set; } = collaboratorName;
    }
}
