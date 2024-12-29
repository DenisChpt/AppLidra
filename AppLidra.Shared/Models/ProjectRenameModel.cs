//-----------------------------------------------------------------------
// <copiright file="ProjectRenameModel.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class ProjectRenameModel(int projectId, string newName)
    {
        public int ProjectId { get; set; } = projectId;
        public string NewName { get; set; } = newName;
    }

}
