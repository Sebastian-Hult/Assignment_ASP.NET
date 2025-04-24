using System.Threading.Tasks;
using Business.Dtos;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class ProjectController(IProjectService projectService, IClientRepository clientRepository, IStatusRepository statusRepository) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<IActionResult> AddProject()
    {
        ViewBag.ErrorMessage = "";

        var clients = await _clientRepository.GetAllAsync();
        var statuses = await _statusRepository.GetAllAsync();

        var model = new AddProjectViewModel
        {
            Form = new AddProjectForm(),
            Clients = clients,
            Statuses = statuses
        };

        return View(model);
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
        
        
        var addProjectForm = new AddProjectForm
        {
            ProjectImage = model.Form.ProjectImage,
            ProjectName = model.Form.ProjectName,
            ClientName = model.Form.ClientName,
            Description = model.Form.Description,
            StartDate = model.Form.StartDate,
            EndDate = model.Form.EndDate,
            Budget = model.Form.Budget,
        };

        var result = await _projectService.CreateProjectAsync(addProjectForm);
        if (result != null)
            return RedirectToAction("Projects", "Admin");
        
        return View(model);
    }

    //public async Task<IActionResult> EditProject(string id)
    //{
    //    var project = await _projectService.GetProjectAsync(id);

    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> EditProject(EditProjectViewModel model)
    //{
    //    return View(model);
    //}

    //public async Task<IActionResult> DeleteProject()
    //{
    //    return View();
    //}
}
