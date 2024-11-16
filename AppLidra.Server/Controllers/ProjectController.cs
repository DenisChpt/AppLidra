using Microsoft.AspNetCore.Mvc;
using AppLidra.Shared.Models;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        // get all projects of a user
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(Project project)
    {
        // get a project from the database
        return Ok();
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProjectById(int projectId)
    {
        // get a specific project (from id)
        return Ok();
    }
}
