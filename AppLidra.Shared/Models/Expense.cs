namespace AppLidra.Shared.Models;

public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string PaidBy { get; set; }
    public Dictionary<string, decimal> Distribution { get; set; } = new Dictionary<string, decimal>();
}
