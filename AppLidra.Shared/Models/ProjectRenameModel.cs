//-----------------------------------------------------------------------
// <copiright file="ProjectRenameModel.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class ProjectRenameModel(int projectId, string newName)
    {
        public int ProjectId { get; set; } = projectId;
        public string NewName { get; set; } = newName;
    }

}
