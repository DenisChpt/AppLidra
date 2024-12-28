using AppLidra.Server.Data;
using AppLidra.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppLidra.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseController(JsonDataStore store) : ControllerBase
{
    JsonDataStore _store = store;

    [HttpGet("{expenseId}")]
    public IActionResult GetExpenses(int expenseId)
    {
        Expense expenses = _store.Expenses.Where(e => e.Id == expenseId).First();
        return Ok(expenses);
    }

    [HttpPost]
    public IActionResult AddExpense([FromBody] ExpenseModel expenseModel)
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        User? user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound("User not found");

        Project? project = _store.Projects.FirstOrDefault(p => p.Id == expenseModel.ProjectId);
        if (project == null)
            return NotFound("Project not found");

        bool hasRights = project.Collaborators.Contains(user.UserName);

        if (!hasRights)
            return NotFound("User not a collaborator");

        for (int i = 0; i < expenseModel.Shares.Count; i++)
        {
            User? shareHolder = _store.Users.FirstOrDefault(u => u.Id == userId);
            if (shareHolder == null)
                return NotFound("ShareHolder not found");

            bool isCollaborator = project.Collaborators.Contains(shareHolder.UserName);
            if (!isCollaborator)
                return NotFound("ShareHolder is not a collaborator");
        }

        int id = _store.Expenses.Count != 0 ? _store.Expenses.Max(e => e.Id) + 1 : 1;
        Expense expense = new(id, expenseModel.Name, expenseModel.Amount, expenseModel.Date, expenseModel.ProjectId, expenseModel.Shares, user.UserName);

        _store.Expenses.Add(expense);
        _store.SaveChanges();

        return Ok(expense);
    }

    [HttpPut("{expenseId}")]
    public IActionResult UpdateExpense(int expenseId, [FromBody] Expense updatedExpense)
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        User? user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound("User not found");

        Expense? expense = _store.Expenses.FirstOrDefault(e => e.Id == expenseId);
        if (expense == null)
            return NotFound("Expense not found");

        Project? project = _store.Projects.FirstOrDefault(p => p.Id == expense.ProjectId);
        if (project == null)
            return NotFound("Project not found");

        bool hasRights = project.Collaborators.Contains(user.UserName);

        if (!hasRights)
            return NotFound("User not a collaborator");

        expense.Name = updatedExpense.Name;
        expense.Author = updatedExpense.Author;
        expense.Amount = updatedExpense.Amount;
        expense.Date = updatedExpense.Date;
        expense.Shares = updatedExpense.Shares;

        _store.SaveChanges();

        return Ok(expense);
    }

    [HttpDelete("{expenseId}")]
    public IActionResult DeleteExpense(int expenseId)
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        User? user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound("User not found");

        Expense? expense = _store.Expenses.FirstOrDefault(e => e.Id == expenseId);
        if (expense == null)
            return NotFound("Expense not found");

        Project? project = _store.Projects.FirstOrDefault(p => p.Id == expense.ProjectId);
        if (project == null)
            return NotFound("Project not found");

        bool hasRights = project.Collaborators.Contains(user.UserName);

        if (!hasRights)
            return NotFound("User not a collaborator");


        _store.Expenses.Remove(expense);

        _store.SaveChanges();

        return Ok("Expense deleted successfully");
    }
}