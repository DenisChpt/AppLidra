namespace AppLidra.Shared.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<Expense> Expenses { get; set; } = new List<Expense>();
}
