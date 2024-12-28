namespace AppLidra.Shared.Models
{
    public class CollaboratorModificationModel
    {
        public int ProjectId { get; set; }
        public string CollaboratorName { get; set; }

        public CollaboratorModificationModel(int projectId, string collaboratorName)
        {
            ProjectId = projectId;
            CollaboratorName = collaboratorName;
        }
    }

}
