//-----------------------------------------------------------------------
// <copiright file="ExpenseController.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Server.Controllers
{
    using System.Globalization;
    using System.Security.Claims;
    using AppLidra.Server.Data;
    using AppLidra.Shared.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing expenses.
    /// </summary>
    /// <param name="store">The data store for expenses.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController(JsonDataStore store) : ControllerBase
    {
        private readonly JsonDataStore _store = store;

        /// <summary>
        /// Retrieves the expense with the specified ID.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to retrieve.</param>
        /// <returns>The expense with the specified ID.</returns>
        [HttpGet("{expenseId}")]
        public IActionResult GetExpenses(int expenseId)
        {
            Expense expenses = this._store.Expenses.First(e => e.Id == expenseId);
            return Ok(expenses);
        }

        /// <summary>
        /// Adds a new expense.
        /// </summary>
        /// <param name="expenseModel">The model of the expense to add.</param>
        /// <returns>The added expense.</returns>
        [HttpPost]
        public IActionResult AddExpense([FromBody] ExpenseModel expenseModel)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);

            User? user = this._store.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == expenseModel.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            string userName = user.UserName ?? string.Empty;
            bool hasRights = project.Collaborators.Contains(userName);

            if (!hasRights)
            {
                return NotFound("User not a collaborator");
            }

            if ((expenseModel is null) || (expenseModel.Shares is null))
            {
                return BadRequest("Invalid expense model");
            }

            for (int i = 0; i < expenseModel.Shares.Count; i++)
            {
                User? shareHolder = this._store.Users.FirstOrDefault(u => u.Id == userId);
                if (shareHolder == null)
                {
                    return NotFound("ShareHolder not found");
                }

                string shareHolderName = shareHolder.UserName ?? string.Empty;
                bool isCollaborator = project.Collaborators.Contains(shareHolderName);
                if (!isCollaborator)
                {
                    return NotFound("ShareHolder is not a collaborator");
                }
            }

            int id = this._store.Expenses.Count != 0 ? this._store.Expenses.Max(e => e.Id) + 1 : 1;
            Expense expense = new (id, expenseModel.Name, expenseModel.Amount, expenseModel.Date, expenseModel.ProjectId, expenseModel.Shares, userName);

            this._store.Expenses.Add(expense);
            this._store.SaveChanges();

            return Ok(expense);
        }

        /// <summary>
        /// Updates an existing expense.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to update.</param>
        /// <param name="updatedExpense">The updated expense details.</param>
        /// <returns>The updated expense.</returns>
        [HttpPut("{expenseId}")]
        public IActionResult UpdateExpense(int expenseId, [FromBody] Expense updatedExpense)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);

            User? user = this._store.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Expense? expense = this._store.Expenses.FirstOrDefault(e => e.Id == expenseId);
            if (expense == null)
            {
                return NotFound("Expense not found");
            }

            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == expense.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            string userName = user.UserName ?? string.Empty;
            bool hasRights = project.Collaborators.Contains(userName);

            if (!hasRights)
            {
                return NotFound("User not a collaborator");
            }

            if (updatedExpense is null)
            {
                return BadRequest("Invalid expense model");
            }

            expense.Name = updatedExpense.Name;
            expense.Author = updatedExpense.Author;
            expense.Amount = updatedExpense.Amount;
            expense.Date = updatedExpense.Date;
            expense.Shares = updatedExpense.Shares;

            this._store.SaveChanges();

            return Ok(expense);
        }

        /// <summary>
        /// Deletes the expense with the specified ID.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete("{expenseId}")]
        public IActionResult DeleteExpense(int expenseId)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);

            User? user = this._store.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Expense? expense = this._store.Expenses.FirstOrDefault(e => e.Id == expenseId);
            if (expense == null)
            {
                return NotFound("Expense not found");
            }

            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == expense.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            string userName = user.UserName ?? string.Empty;
            bool hasRights = project.Collaborators.Contains(userName);

            if (!hasRights)
            {
                return NotFound("User not a collaborator");
            }

            _ = this._store.Expenses.Remove(expense);

            this._store.SaveChanges();

            return Ok("Expense deleted successfully");
        }
    }
}