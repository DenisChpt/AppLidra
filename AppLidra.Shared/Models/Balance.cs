//-----------------------------------------------------------------------
// <copiright file="Balance.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a balance with a list of balance parts.
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Gets or sets the list of balance parts.
        /// </summary>
        public List<BalancePart> BalanceParts { get; set; } = [];
    }
}