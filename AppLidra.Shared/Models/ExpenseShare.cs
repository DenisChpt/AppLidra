//-----------------------------------------------------------------------
// <copiright file="ExpenseShare.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents a share of an expense for a user.
    /// </summary>
    public class ExpenseShare
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseShare"/> class.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="share">The share of the expense.</param>
        public ExpenseShare(string userName, double share)
        {
            if (share is < 0 or > 1)
            {
                throw new ArgumentException("share out of bounds.");
            }

            this.UserName = userName;
            this.Share = share;
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the share of the expense.
        /// </summary>
        public double Share { get; set; }
    }
}
