//-----------------------------------------------------------------------
// <copiright file="DistributionPart.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DistributionPart"/> class.
    /// </summary>
    /// <param name="name">The name of the distribution part.</param>
    /// <param name="share">The share of the distribution part.</param>
    public class DistributionPart(string name, double share)
    {
        /// <summary>
        /// Gets or sets the name of the distribution part.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Gets or sets the share of the distribution part.
        /// </summary>
        public double Share { get; set; } = share;
    }
}