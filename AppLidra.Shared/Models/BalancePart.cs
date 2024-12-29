//-----------------------------------------------------------------------
// <copiright file="BalancePart.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    public class BalancePart(string name, double amount)
    {
        public string Name { get; set; } = name;
        public double Amount { get; set; } = amount;
    }
}
