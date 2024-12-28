namespace AppLidra.Shared.Models
{
    public class ProjectRenameModel
    {
        public int ProjectId { get; set; }
        public string NewName { get; set; }

        public ProjectRenameModel ( int projectId, string newName)
        {
            ProjectId = projectId;
            NewName = newName;
        }
    }

}
