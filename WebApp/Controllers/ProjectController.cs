using Business.Dtos;
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
public class ProjectController(IProjectService projectService, IClientRepository clientRepository, IStatusRepository statusRepository, IAuthService authService, IUserRepository userRepository) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IAuthService _authService = authService;
    private readonly IUserRepository _userRepository = userRepository;

    //[Authorize]
    public async Task<IActionResult> Index()
    {
        var clients = await _clientRepository.GetAllAsync();
        var statuses = await _statusRepository.GetAllAsync();

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

        //var userId = await _authService.GetUserIdAsync(User);

        //pvm.AddProject.Form.UserId = userId;
        //pvm.AddProject.Form.StatusId = 1;



        return View(pvm);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        var client = await _clientRepository.GetAsync(x => x.Id == model.Form.ClientId);
        var userId = await _authService.GetUserIdAsync(User);

        if (client != null)
            model.Form.ClientName = client.ClientName;

        model.Form.StatusId = 2;
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

        ViewBag.ExistingProject = existingProject;

        existingProject.UserId = model.Form.UserId!;

        var client = await _clientRepository.GetAsync(x => x.Id == model.Form.ClientId);

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

        if (!string.IsNullOrEmpty(model.Form.ProjectImage))
            existingProject.ProjectImage = model.Form.ProjectImage;

        if (!string.IsNullOrEmpty(model.Form.ProjectName))
            existingProject.ProjectName = model.Form.ProjectName;

        if (!string.IsNullOrEmpty(model.Form.ClientId))
            existingProject.ClientId = model.Form.ClientId;

        if (!string.IsNullOrEmpty(model.Form.Description))
            existingProject.Description = model.Form.Description;

        //if (model.Form.StartDate.HasValue)
        //    existingProject.StartDate = model.Form.StartDate.Value;

        if (model.Form.EndDate.HasValue)
            existingProject.EndDate = model.Form.EndDate;

        if (model.Form.Budget.HasValue)
            existingProject.Budget = model.Form.Budget;

        //if (model.Form.StatusId.HasValue)
        //    existingProject.StatusId = model.Form.StatusId.Value;

        if (existingProject.ClientId == model.Form.ClientId)
            existingProject.ClientName = model.Form.ClientName!;

        if (existingProject.StatusId == model.Form.StatusId)
            existingProject.Status = model.Form.Status!;

        //var editProjectForm = new EditProjectForm
        //{
        //    ProjectImage = model.Form.ProjectImage,
        //    ProjectName = model.Form.ProjectName,
        //    ClientName = model.Form.ClientName,
        //    Description = model.Form.Description,
        //    StartDate = model.Form.StartDate,
        //    EndDate = model.Form.EndDate,
        //    Budget = model.Form.Budget,
        //    Status = model.Form.Status,
        //};

        //var result = await _projectService.UpdateProjectAsync(existingProject);
        //if (result == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );

            return BadRequest(new { success = false, errors });
        }

        //return Ok();
    }

    public async Task<IActionResult> DeleteProject(string id)
    {
        await _projectService.DeleteProjectAsync(id);

        return Ok();
    }
}
