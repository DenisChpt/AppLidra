using Microsoft.AspNetCore.Mvc;
using AppLidra.Shared.Models;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetExpenses(int projectId)
    {
        // get expenses of a project
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense(Expense expense)
    {
        // add an expense to the database
        return Ok();
    }
}
