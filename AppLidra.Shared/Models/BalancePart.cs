namespace AppLidra.Shared.Models;

public class BalancePart(string name, double amount)
{
    public string Name { get; set; } = name;
    public double Amount { get; set; } = amount;
}
