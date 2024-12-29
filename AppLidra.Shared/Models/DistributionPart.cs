namespace AppLidra.Shared.Models
{
    public class DistributionPart(string name, double share)
    {
        public string Name { get; set; } = name;
        public double Share { get; set; } = share;
    }
}