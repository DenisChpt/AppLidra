//-----------------------------------------------------------------------
// <copiright file="Distribution.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a distribution containing multiple distribution parts.
    /// </summary>
    public class Distribution
    {
        /// <summary>
        /// Gets or sets the distribution parts.
        /// </summary>
        public List<DistributionPart> DistributionParts { get; set; } = [];
    }
}