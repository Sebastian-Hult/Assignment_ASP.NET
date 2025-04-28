using System.Threading.Tasks;
using Business.Dtos;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers;

public class ProjectController(IProjectService projectService, IClientRepository clientRepository, IStatusRepository statusRepository, IAuthService authService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IAuthService _authService = authService;

    public async Task<IActionResult> Index()
    {
        var pvm = new ProjectsViewModel
        {
            Projects = await _projectService.GetProjectsAsync(),
            AddProject = new AddProjectViewModel
            {
                Clients = await _clientRepository.GetAllAsync(),
            },
            EditProject = new EditProjectViewModel
            {
                Clients = await _clientRepository.GetAllAsync(),
                Statuses = await _statusRepository.GetAllAsync(),
            }
        };
        return View(pvm);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );

            return BadRequest(new { success = false, errors });
        }

        var client = await _clientRepository.GetAsync(x => x.ClientName == model.Form.ClientName);
        var userId = await _authService.GetUserIdAsync(User);

        if (client == null)
            return View(model);

        if (userId == null)
            return View(model);

        var addProjectForm = new AddProjectForm
        {
            ProjectImage = model.Form.ProjectImage,
            ProjectName = model.Form.ProjectName,
            ClientName = model.Form.ClientName,
            Description = model.Form.Description,
            StartDate = model.Form.StartDate,
            EndDate = model.Form.EndDate,
            Budget = model.Form.Budget,
            ClientId = client.Id,
            UserId = userId,
            StatusId = 1,
        };

        var result = await _projectService.CreateProjectAsync(addProjectForm);
        if (result == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );

            return BadRequest(new { success = false, errors });
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );

            return BadRequest(new { success = false, errors });
        }


        var editProjectForm = new EditProjectForm
        {
            ProjectImage = model.Form.ProjectImage,
            ProjectName = model.Form.ProjectName,
            ClientName = model.Form.ClientName,
            Description = model.Form.Description,
            StartDate = model.Form.StartDate,
            EndDate = model.Form.EndDate,
            Budget = model.Form.Budget,
            Status = model.Form.Status,
        };

        var result = await _projectService.UpdateProjectAsync(editProjectForm);
        if (result == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );

            return BadRequest(new { success = false, errors });
        }

        return Ok();
    }

    public async Task<IActionResult> DeleteProject(string id)
    {
        await _projectService.DeleteProjectAsync(id);

        return Ok();
    }
}
