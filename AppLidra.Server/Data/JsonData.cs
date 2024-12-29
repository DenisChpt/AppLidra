//-----------------------------------------------------------------------
// <copiright file="JsonData.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Server.Data
{
    using AppLidra.Shared.Models;

    /// <summary>
    /// Represents the JSON data containing lists of projects, users, and expenses.
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonData"/> class.
        /// </summary>
        /// <param name="projects">The list of projects.</param>
        /// <param name="users">The list of users.</param>
        /// <param name="expenses">The list of expenses.</param>
        public JsonData(List<Project> projects, List<User> users, List<Expense> expenses)
        {
            Projects = projects ?? [];
            Users = users ?? [];
            Expenses = expenses ?? [];
        }

        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        public List<Project> Projects { get; } = [];

        /// <summary>
        /// Gets the list of users.
        /// </summary>
        public List<User> Users { get; } = [];

        /// <summary>
        /// Gets the list of expenses.
        /// </summary>
        public List<Expense> Expenses { get; } = [];
    }
}