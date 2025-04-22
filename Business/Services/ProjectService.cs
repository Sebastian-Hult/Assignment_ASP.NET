using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Models;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(AddProjectForm form);
    Task<Project> GetProjectAsync(string id);
    Task<IEnumerable<Project>> GetProjectsAsync();
}

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Project> CreateProjectAsync(AddProjectForm form)
    {
        var entity = await _projectRepository.GetAsync(x => x.ProjectName == form.ProjectName);
        if (entity != null)
        {
            Debug.WriteLine("Project already exists");
            return ProjectFactory.CreateProject(entity);
        }

        entity = await _projectRepository.AddAsync(ProjectFactory.CreateProject(form));

        return ProjectFactory.CreateProject(entity);
    }

    public async Task<IEnumerable<Project>> GetProjectsAsync()
    {
        var entities = await _projectRepository.GetAllAsync();
        var projects = entities.Select(ProjectFactory.CreateProject);
        return projects ?? [];
    }

    public async Task<Project> GetProjectAsync(string id)
    {
        var entity = await _projectRepository.GetAsync(x => x.Id == id);
        var project = ProjectFactory.CreateProject(entity!);

        return project ?? null!;
    }

    //public async Task<Project?> UpdateProjectAsync(EditProjectForm form)
    //{
    //    var entity = await _projectRepository.UpdateAsync(x => x.ProjectName == form.ProjectName, ProjectFactory.UpdateProject(form));
    //    var project = ProjectFactory.CreateProject(entity);
    //    return project ?? null!;
    //}
}
