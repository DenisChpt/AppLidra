using AppLidra.Server.Data;
using AppLidra.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace AppLidra.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController(JsonDataStore store) : ControllerBase
    {
        private readonly JsonDataStore _store = store ?? throw new ArgumentNullException(nameof(store));

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User user = _store.Users.First(u => u.Id == userId);
            string userName = user.UserName ?? string.Empty;
            List<Project> projects = _store.Projects.Where(p => p.OwnerUserId == userId || p.Collaborators.Contains(userName)).ToList();
            return Ok(projects);
        }

        [HttpGet("is-owner/{projectId}")]
        public IActionResult IsOwner(int projectId)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            bool res = _store.Projects.Exists(p => p.Id == projectId && p.OwnerUserId == userId);
            return Ok(res);
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] Project project)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User user = _store.Users.First(u => u.Id == userId);
            project.Id = _store.Projects.Count != 0 ? _store.Projects.Max(p => p.Id) + 1 : 1;
            project.OwnerUserId = userId;
            project.CreatedAt = DateTime.UtcNow;
            project.Collaborators = [user.UserName];

            _store.Projects.Add(project);
            _store.SaveChanges();

            return Ok(project);
        }

        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == projectId);
            return project == null ? NotFound("Project not found") : Ok(project);
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            _ = _store.Projects.Remove(project);
            _ = _store.Expenses.RemoveAll(e => e.ProjectId == projectId);
            _store.SaveChanges();

            return Ok("Project and associated expenses deleted successfully");
        }

        [HttpPost("add-collaborator")]
        public IActionResult AddCollaborator(CollaboratorModificationModel collabInfo)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }


            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            if (userId != project.OwnerUserId)
            {
                return NotFound("User demanding is not the owner of the project");
            }


            if (_store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName) == null)
            {
                return NotFound("User not found");
            }


            if (project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return BadRequest("Collaborator already exists in the project.");
            }


            project.Collaborators.Add(collabInfo.CollaboratorName);

            _store.SaveChanges();

            return Ok("Collaborator added.");
        }
        [HttpPost("remove-collaborator")]
        public IActionResult RemoveCollaborator(CollaboratorModificationModel collabInfo)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }


            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            if (userId != project.OwnerUserId)
            {
                return NotFound("User demanding is not the owner of the project.");
            }


            User? collaboratorToRemove = _store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName);
            if (collaboratorToRemove is null)
            {
                return NotFound("User not found.");
            }


            if (collaboratorToRemove.Id == project.OwnerUserId)
            {
                return NotFound("The project owner has to be a collabaorator.");
            }


            if (!project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return BadRequest("Collaborator does not exists in the project.");
            }


            _ = project.Collaborators.Remove(collabInfo.CollaboratorName);
            bool isShareHolder;

            // share out expense on remaining collaborators
            _ = _store.Expenses.RemoveAll(e => e.Id == collabInfo.ProjectId);

            List<Expense> expenses = _store.Expenses.Where(e => e.Id == collabInfo.ProjectId).ToList();
            double shareStock = 0;

            for (int i = 0; i < expenses.Count; i++)
            {
                isShareHolder = false;
                for (int j = 0; j < expenses[i].Shares.Count; j++)
                {
                    if (expenses[i].Shares[j].UserName == collabInfo.CollaboratorName)
                    {
                        isShareHolder = true;
                        shareStock = expenses[i].Shares[j].Share;
                        expenses[i].Shares.RemoveAt(j);
                        break;
                    }
                }
                if (isShareHolder)
                {
                    for (int j = 0; j < expenses[i].Shares.Count; j++)
                    {
                        expenses[i].Shares[j].Share += shareStock / expenses[i].Shares.Count;
                    }

                }

                _store.Expenses.Add(expenses[i]);
            }

            _store.SaveChanges();

            return Ok("Collaborator removed.");
        }

        [HttpPost("quit-project")]
        public IActionResult QuitProject(CollaboratorModificationModel collabInfo)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }


            User? collaboratorToRemove = _store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName);
            if (collaboratorToRemove is null)
            {
                return NotFound("User not found.");
            }


            if (collaboratorToRemove.Id == project.OwnerUserId)
            {
                return NotFound("The project owner has to be a collabaorator.");
            }


            if (!project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return BadRequest("Collaborator does not exists in the project.");
            }


            _ = project.Collaborators.Remove(collabInfo.CollaboratorName);
            bool isShareHolder;

            // share out expense on remaining collaborators
            _ = _store.Expenses.RemoveAll(e => e.Id == collabInfo.ProjectId);

            List<Expense> expenses = _store.Expenses.Where(e => e.Id == collabInfo.ProjectId).ToList();
            double shareStock = 0;

            for (int i = 0; i < expenses.Count; i++)
            {
                isShareHolder = false;
                for (int j = 0; j < expenses[i].Shares.Count; j++)
                {
                    if (expenses[i].Shares[j].UserName == collabInfo.CollaboratorName)
                    {
                        isShareHolder = true;
                        shareStock = expenses[i].Shares[j].Share;
                        expenses[i].Shares.RemoveAt(j);
                        break;
                    }
                }
                if (isShareHolder)
                {
                    for (int j = 0; j < expenses[i].Shares.Count; j++)
                    {
                        expenses[i].Shares[j].Share += shareStock / expenses[i].Shares.Count;
                    }

                }

                _store.Expenses.Add(expenses[i]);
            }

            _store.SaveChanges();

            return Ok("Collaborator removed.");
        }

        [HttpPost("rename")]
        public IActionResult RenameProject(ProjectRenameModel newProjectInfos)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == newProjectInfos.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }


            project.Name = newProjectInfos.NewName;

            _store.SaveChanges();

            return Ok("Collaborator added.");
        }

        [HttpGet("expenses/{projectId}")]
        public IActionResult GetExpenses(int projectId)
        {
            List<Expense> expenses = store.Expenses.Where(e => e.ProjectId == projectId).ToList();
            return Ok(expenses);
        }

        [HttpGet("collaborators/{projectId}")]
        public IActionResult GetCollaborators(int projectId)
        {
            List<string> collaborators = store.Projects.First(p => p.Id == projectId).Collaborators;
            return Ok(collaborators);
        }

        [HttpGet("distribution/{projectId}")]
        public IActionResult GetDistribution(int projectId)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }


            List<Expense> expenses = _store.Expenses.Where(e => e.ProjectId == projectId).ToList();
            double totalAmount = expenses.Sum(e => e.Amount);

            Distribution distribution = new();

            if (totalAmount == 0)
            {
                return Ok(distribution);
            }


            List<DistributionPart> groupedExpenses = expenses
                .GroupBy(e => e.Author)
                .Select(group => new DistributionPart(
                    name: group.Key,
                    share: group.Sum(e => e.Amount) / totalAmount
                ))
                .ToList();

            distribution.DistributionParts = groupedExpenses;

            return Ok(distribution);
        }

        [HttpGet("balance/{projectId}")]
        public IActionResult GetBalance(int projectId)
        {
            Project? project = _store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }


            List<Expense> expenses = _store.Expenses.Where(e => e.ProjectId == projectId).ToList();

            Dictionary<string, double> balances = [];

            // Credits (payed amounts)
            foreach (Expense expense in expenses)
            {
                if (!balances.ContainsKey(expense.Author))
                {
                    balances[expense.Author] = 0;
                }

                balances[expense.Author] += expense.Amount;
            }

            // Debits (dued amounts according to shares)
            foreach (Expense expense in expenses)
            {
                foreach (ExpenseShare share in expense.Shares)
                {
                    if (!balances.ContainsKey(share.UserName))
                    {
                        balances[share.UserName] = 0;
                    }
                    balances[share.UserName] -= expense.Amount * share.Share;
                }
            }

            // constructs the results
            Balance balance = new()
            {
                BalanceParts = balances
                    .Select(b => new BalancePart(
                        name: b.Key,
                        amount: Math.Round(b.Value, 2)
                    ))
                    .ToList()
            };

            return Ok(balance);
        }
    }
}