//-----------------------------------------------------------------------
// <copiright file="Project.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
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
