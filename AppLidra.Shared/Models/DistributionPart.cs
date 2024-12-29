//-----------------------------------------------------------------------
// <copiright file="DistributionPart.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class DistributionPart(string name, double share)
    {
        public string Name { get; set; } = name;
        public double Share { get; set; } = share;
    }
}