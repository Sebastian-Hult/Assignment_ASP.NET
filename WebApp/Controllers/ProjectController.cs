using System.Runtime.CompilerServices;
using Business.Dtos;
using Business.Factories;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers;

//[Authorize]
public class ProjectController(IProjectService projectService, IClientRepository clientRepository, IStatusRepository statusRepository, IAuthService authService, IUserRepository userRepository, IProjectRepository projectRepository) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IAuthService _authService = authService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    //[Authorize]
    public async Task<IActionResult> Index(int? status, string id)
    {
        var clients = await _clientRepository.GetAllAsync();
        var statuses = await _statusRepository.GetAllAsync();
        //var project = await _projectService.GetProjectAsync(id);
        //var project = await _projectRepository.GetAsync(x => x.Id == id);

        var selectListItemClient = clients
            .Select(x => new SelectListItem
            {
                Text = x.ClientName,
                Value = x.Id.ToString()
            })
            .ToList();

        var selectListItemStatus = statuses
            .Select(x => new SelectListItem
            {
                Text = x.StatusName,
                Value = x.Id.ToString()
            })
            .ToList();

        ViewBag.Clients = selectListItemClient;
        ViewBag.Statuses = selectListItemStatus;

        var projects = await _projectService.GetProjectsAsync();
        //var projects = await _projectRepository.GetAllAsync();
        var filteredProjects = projects;

        if (status.HasValue)
            filteredProjects = projects.Where(x => x.StatusId == status.Value);

        ViewBag.Projects = projects.ToList();
        ViewBag.CurrentStatus = status;

        var pvm = new ProjectsViewModel
        {
            Projects = filteredProjects,
            AddProject = new AddProjectViewModel
            {
                Clients = await _clientRepository.GetAllAsync(),
            },
            EditProject = new EditProjectViewModel
            {
                //Form = new EditProjectForm()
                //{
                //    Id = project.Id
                //},
                //Projects = await _projectService.GetProjectsAsync(),
                //Projects = await _projectRepository.GetAllAsync(),
                Clients = await _clientRepository.GetAllAsync(),
                Statuses = await _statusRepository.GetAllAsync(),
            }
        };

        return View(pvm);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        var client = await _clientRepository.GetAsync(x => x.Id == model.Form.ClientId);
        var userId = await _authService.GetUserIdAsync(User);

        if (client != null)
            model.Form.ClientName = client.ClientName;

        model.Form.StatusId = 1;
        model.Form.UserId = userId;

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

        var addProjectForm = new AddProjectForm
        {
            ProjectImage = model.Form.ProjectImage,
            ProjectName = model.Form.ProjectName,
            ClientName = model.Form.ClientName,
            Description = model.Form.Description,
            StartDate = model.Form.StartDate,
            EndDate = model.Form.EndDate,
            Budget = model.Form.Budget,
            ClientId = model.Form.ClientId,
            UserId = model.Form.UserId,
            StatusId = model.Form.StatusId,
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
        var existingProject = await _projectService.GetProjectAsync(model.Form.Id);
        //var existingProject = await _projectRepository.GetAsync(x => x.Id == model.Form.Id);
        var client = await _clientRepository.GetAsync(x => x.Id == model.Form.ClientId);
        var status = await _statusRepository.GetAsync(x => x.Id == existingProject.StatusId);

        if (client != null)
            model.Form.ClientName = client.ClientName;

        if (status != null)
            model.Form.Status = status.StatusName;

        if (existingProject == null)
            return NotFound();

        model.Form.UserId = existingProject.UserId;

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
            Id = model.Form.Id,

            ProjectImage = !string.IsNullOrWhiteSpace(model.Form.ProjectImage)
                ? model.Form.ProjectImage
                : existingProject.ProjectImage,
            //: existingProject.Image,


            ProjectName = !string.IsNullOrWhiteSpace(model.Form.ProjectName)
                ? model.Form.ProjectName
                : existingProject.ProjectName,


            ClientId = !string.IsNullOrWhiteSpace(model.Form.ClientId)
                ? model.Form.ClientId
                : existingProject.ClientId,


            ClientName = model.Form.ClientId != existingProject.ClientId
                ? model.Form.ClientName
                : existingProject.ClientName,
                //: existingProject.Client.ClientName,


            Description = !string.IsNullOrWhiteSpace(model.Form.Description)
                ? model.Form.Description
                : existingProject.Description,


            StartDate = model.Form.StartDate != default
                ? model.Form.StartDate
                : existingProject.StartDate,


            EndDate = model.Form.EndDate ?? existingProject.EndDate,


            Budget = model.Form.Budget ?? existingProject.Budget,


            StatusId = model.Form.StatusId > 0
                ? model.Form.StatusId
                : existingProject.StatusId,


            Status = model.Form.StatusId != existingProject.StatusId
                ? model.Form.Status
                : existingProject.Status,
                //: existingProject.Status.StatusName,


            UserId = model.Form.UserId,
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
