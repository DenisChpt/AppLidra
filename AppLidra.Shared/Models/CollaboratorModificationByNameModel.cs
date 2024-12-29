//-----------------------------------------------------------------------
// <copiright file="CollaboratorModificationByNameModel.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class CollaboratorModificationModel(int projectId, string collaboratorName)
    {
        public int ProjectId { get; set; } = projectId;
        public string CollaboratorName { get; set; } = collaboratorName;
    }
}
