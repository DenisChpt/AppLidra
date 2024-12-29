//-----------------------------------------------------------------------
// <copiright file="CollaboratorModificationByNameModel.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class CollaboratorModificationModel(int projectId, string collaboratorName)
    {
        public int ProjectId { get; set; } = projectId;
        public string CollaboratorName { get; set; } = collaboratorName;
    }
}
