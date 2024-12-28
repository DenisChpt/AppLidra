namespace AppLidra.Shared.Models;

public class BalancePart
{
    public string Name { get; set; }
    public double Amount { get; set; }

    public BalancePart(string name, double amount)
    {
        Name = name;
        Amount = amount;
    }
}
