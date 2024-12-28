namespace AppLidra.Shared.Models;

public class DistributionPart
{
    public string Name { get; set; }
    public double Share { get; set; }

    public DistributionPart(string name, double share)
    {
        Name = name;
        Share = share;
    }
}