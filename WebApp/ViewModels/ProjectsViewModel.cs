using Business.Models;
using Data.Entities;

namespace WebApp.ViewModels;

public class ProjectsViewModel
{
    public IEnumerable<Project> Projects { get; set; } = [];

    //public IEnumerable<ProjectEntity> Projects { get; set; } = [];
    public AddProjectViewModel AddProject { get; set; } = new AddProjectViewModel();
    public EditProjectViewModel EditProject { get; set; } = new EditProjectViewModel();
}
