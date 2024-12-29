//-----------------------------------------------------------------------
// <copiright file="ExpenseModel.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Shared.Models
{
    /// <summary>
    /// Represents an expense with a name, amount, date, project ID, and shares.
    /// </summary>
    public class ExpenseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseModel"/> class.
        /// </summary>
        /// <param name="name">The name of the expense.</param>
        /// <param name="amount">The amount of the expense.</param>
        /// <param name="date">The date of the expense.</param>
        /// <param name="projectId">The project ID associated with the expense.</param>
        /// <param name="shares">The list of expense shares.</param>
        /// <exception cref="ArgumentException">Thrown when the shares do not add up to 1.</exception>
        public ExpenseModel(string name, double amount, DateTime date, int projectId, List<ExpenseShare> shares)
        {
            this.Name = name;
            this.Amount = amount;
            this.Date = date;
            this.ProjectId = projectId;
            double sharesCount = 0;
            ArgumentNullException.ThrowIfNull(shares, nameof(shares));

            for (int i = 0; i < shares.Count; i++)
            {
                sharesCount += shares[i].Share;
            }

            if (sharesCount != 1)
            {
                throw new ArgumentException("Shares do not add up to 1");
            }

            this.Shares = shares;
        }

        /// <summary>
        /// Gets or sets the name of the expense.
        /// </summary>
        public string Name { get; set; } = "New Expense";

        /// <summary>
        /// Gets or sets the amount of the expense.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the date of the expense.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the project ID associated with the expense.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets the list of expense shares.
        /// </summary>
        public List<ExpenseShare> Shares { get; } = [];
    }
}
