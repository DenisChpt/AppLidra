//-----------------------------------------------------------------------
// <copiright file="BalancePart.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BalancePart"/> class.
    /// </summary>
    /// <param name="name">The name of the balance part.</param>
    /// <param name="amount">The amount of the balance part.</param>
    public class BalancePart(string name, double amount)
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public double Amount { get; set; } = amount;
    }
}
