//-----------------------------------------------------------------------
// <copiright file="ProjectController.cs">
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
    /// Controller for managing projects.
    /// </summary>
    /// <param name="store">The data store for projects.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController(JsonDataStore store) : ControllerBase
    {
        private readonly JsonDataStore _store = store ?? throw new ArgumentNullException(nameof(store));

        /// <summary>
        /// Gets all projects for the current user.
        /// </summary>
        /// <returns>A list of projects.</returns>
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User user = this._store.Users.First(u => u.Id == userId);
            string userName = user.UserName ?? string.Empty;
            List<Project> projects = this._store.Projects.Where(p => p.OwnerUserId == userId || p.Collaborators.Contains(userName)).ToList();
            return this.Ok(projects);
        }

        /// <summary>
        /// Checks if the current user is the owner of the specified project.
        /// </summary>
        /// <param name="projectId">The ID of the project to check ownership for.</param>
        /// <returns>True if the current user is the owner, otherwise false.</returns>
        [HttpGet("is-owner/{projectId}")]
        public IActionResult IsOwner(int projectId)
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            bool res = this._store.Projects.Exists(p => p.Id == projectId && p.OwnerUserId == userId);
            return this.Ok(res);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">The project to create.</param>
        /// <returns>The created project.</returns>
        [HttpPost]
        public IActionResult CreateProject([FromBody] Project project)
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            User user = this._store.Users.First(u => u.Id == userId);
            if (project is null)
            {
                return this.BadRequest("Project is null");
            }

            project.Id = this._store.Projects.Count != 0 ? this._store.Projects.Max(p => p.Id) + 1 : 1;
            project.OwnerUserId = userId;
            project.CreatedAt = DateTime.UtcNow;
            project.Collaborators = [user.UserName];

            this._store.Projects.Add(project);
            this._store.SaveChanges();

            return this.Ok(project);
        }

        /// <summary>
        /// Gets a project by its ID.
        /// </summary>
        /// <param name="projectId">The ID of the project to retrieve.</param>
        /// <returns>The project with the specified ID.</returns>
        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == projectId);
            return project == null ? this.NotFound("Project not found") : this.Ok(project);
        }

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="projectId">The ID of the project to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return this.NotFound("Project not found");
            }

            _ = this._store.Projects.Remove(project);
            _ = this._store.Expenses.RemoveAll(e => e.ProjectId == projectId);
            this._store.SaveChanges();

            return this.Ok("Project and associated expenses deleted successfully");
        }

        /// <summary>
        /// Adds a collaborator to a project.
        /// </summary>
        /// <param name="collabInfo">The collaborator modification model containing project ID and collaborator name.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost("add-collaborator")]
        public IActionResult AddCollaborator(CollaboratorModificationModel collabInfo)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return this.NotFound("Project not found");
            }

            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            if (userId != project.OwnerUserId)
            {
                return this.NotFound("User demanding is not the owner of the project");
            }

            if (this._store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName) == null)
            {
                return this.NotFound("User not found");
            }

            if (collabInfo is null)
            {
                return this.BadRequest("Collaborator infos are null");
            }

            if (project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return this.BadRequest("Collaborator already exists in the project.");
            }

            project.Collaborators.Add(collabInfo.CollaboratorName);

            this._store.SaveChanges();

            return this.Ok("Collaborator added.");
        }

        /// <summary>
        /// Removes a collaborator from a project.
        /// </summary>
        /// <param name="collabInfo">The collaborator modification model containing project ID and collaborator name.</param>
        /// <returns>The result of the remove operation.</returns>
        [HttpPost("remove-collaborator")]
        public IActionResult RemoveCollaborator(CollaboratorModificationModel collabInfo)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return this.NotFound("Project not found.");
            }

            int userId = int.Parse(this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
            if (userId != project.OwnerUserId)
            {
                return this.NotFound("User demanding is not the owner of the project.");
            }

            User? collaboratorToRemove = this._store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName);
            if (collaboratorToRemove is null)
            {
                return this.NotFound("User not found.");
            }

            if (collaboratorToRemove.Id == project.OwnerUserId)
            {
                return this.NotFound("The project owner has to be a collabaorator.");
            }

            if (collabInfo is null)
            {
                return this.BadRequest("Collaborator infos are null.");
            }

            if (!project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return this.BadRequest("Collaborator does not exists in the project.");
            }

            _ = project.Collaborators.Remove(collabInfo.CollaboratorName);
            bool isShareHolder;

            // share out expense on remaining collaborators
            List<Expense> expenses = this._store.Expenses.Where(e => e.ProjectId == collabInfo.ProjectId).ToList();
            _ = this._store.Expenses.RemoveAll(e => e.ProjectId == collabInfo.ProjectId);

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

                this._store.Expenses.Add(expenses[i]);
            }

            this._store.SaveChanges();

            return this.Ok("Collaborator removed.");
        }

        /// <summary>
        /// Allows a collaborator to quit a project.
        /// </summary>
        /// <param name="collabInfo">The collaborator modification model containing project ID and collaborator name.</param>
        /// <returns>The result of the quit operation.</returns>
        [HttpPost("quit-project")]
        public IActionResult QuitProject(CollaboratorModificationModel collabInfo)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == collabInfo.ProjectId);
            if (project == null)
            {
                return this.NotFound("Project not found.");
            }

            User? collaboratorToRemove = this._store.Users.FirstOrDefault(u => u.UserName == collabInfo.CollaboratorName);
            if (collaboratorToRemove is null)
            {
                return this.NotFound("User not found.");
            }

            if (collaboratorToRemove.Id == project.OwnerUserId)
            {
                return this.NotFound("The project owner has to be a collabaorator.");
            }

            if (collabInfo is null)
            {
                return this.BadRequest("Collaborator infos are null.");
            }

            if (!project.Collaborators.Contains(collabInfo.CollaboratorName))
            {
                return this.BadRequest("Collaborator does not exists in the project.");
            }

            _ = project.Collaborators.Remove(collabInfo.CollaboratorName);
            bool isShareHolder;

            // share out expense on remaining collaborators
            _ = this._store.Expenses.RemoveAll(e => e.Id == collabInfo.ProjectId);

            List<Expense> expenses = this._store.Expenses.Where(e => e.Id == collabInfo.ProjectId).ToList();
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

                this._store.Expenses.Add(expenses[i]);
            }

            this._store.SaveChanges();

            return this.Ok("Collaborator removed.");
        }

        /// <summary>
        /// Renames a project.
        /// </summary>
        /// <param name="newProjectInfos">The new project information.</param>
        /// <returns>The result of the rename operation.</returns>
        [HttpPost("rename")]
        public IActionResult RenameProject(ProjectRenameModel newProjectInfos)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == newProjectInfos.ProjectId);
            if (project == null)
            {
                return this.NotFound("Project not found");
            }

            if (newProjectInfos is null)
            {
                return this.BadRequest("New project infos are null");
            }

            project.Name = newProjectInfos.NewName;

            this._store.SaveChanges();

            return this.Ok("Collaborator added.");
        }

        /// <summary>
        /// Gets the list of expenses for a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>The list of expenses.</returns>
        [HttpGet("expenses/{projectId}")]
        public IActionResult GetExpenses(int projectId)
        {
            List<Expense> expenses = store.Expenses.Where(e => e.ProjectId == projectId).ToList();
            return this.Ok(expenses);
        }

        /// <summary>
        /// Gets the list of collaborators for a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>The list of collaborators.</returns>
        [HttpGet("collaborators/{projectId}")]
        public IActionResult GetCollaborators(int projectId)
        {
            List<string> collaborators = store.Projects.First(p => p.Id == projectId).Collaborators;
            return this.Ok(collaborators);
        }

        /// <summary>
        /// Gets the distribution of expenses for a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>The distribution of expenses.</returns>
        [HttpGet("distribution/{projectId}")]
        public IActionResult GetDistribution(int projectId)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return this.NotFound("Project not found");
            }

            List<Expense> expenses = this._store.Expenses.Where(e => e.ProjectId == projectId).ToList();
            double totalAmount = expenses.Sum(e => e.Amount);

            Distribution distribution = new ();

            if (totalAmount == 0)
            {
                return this.Ok(distribution);
            }

            List<DistributionPart> groupedExpenses = expenses
                .GroupBy(e => e.Author)
                .Select(group => new DistributionPart(
                    name: group.Key,
                    share: group.Sum(e => e.Amount) / totalAmount))
                .ToList();

            distribution.DistributionParts = groupedExpenses;

            return this.Ok(distribution);
        }

        /// <summary>
        /// Gets the balance for a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>The balance of the project.</returns>
        [HttpGet("balance/{projectId}")]
        public IActionResult GetBalance(int projectId)
        {
            Project? project = this._store.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return this.NotFound("Project not found");
            }

            List<Expense> expenses = this._store.Expenses.Where(e => e.ProjectId == projectId).ToList();

            Dictionary<string, double> balances = [];

            // Credits (paid amounts)
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
            Balance balance = new ()
            {
                BalanceParts = balances.Select(b => new BalancePart(name: b.Key, amount: Math.Round(b.Value, 2))).ToList(),
            };

            return this.Ok(balance);
        }
    }
}