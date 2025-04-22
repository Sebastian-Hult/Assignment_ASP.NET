using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class ProjectController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    public IActionResult AddProject()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        return View(model);
    }

    public IActionResult EditProject()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectViewModel model)
    {
        return View(model);
    }

    public async Task<IActionResult> DeleteProject()
    {
        return View();
    }
}
