using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Models;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectEntity> CreateProjectAsync(AddProjectForm form);
    Task<bool> DeleteProjectAsync(string id);
    Task<Project> GetProjectAsync(string id);
    Task<IEnumerable<Project>> GetProjectsAsync();
    Task<Project> UpdateProjectAsync(EditProjectForm form);
}

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<ProjectEntity> CreateProjectAsync(AddProjectForm form)
    {
        var existing = await _projectRepository.GetAsync(x => x.ProjectName == form.ProjectName);
        if (existing != null)
        {
            Debug.WriteLine("Project already exists");
            return existing;
        }

        var entity = await _projectRepository.AddAsync(ProjectFactory.CreateProject(form));

        return entity;
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

    public async Task<Project> UpdateProjectAsync(EditProjectForm form)
    {
        var entity = await _projectRepository.UpdateAsync(x => x.ProjectName == form.ProjectName, ProjectFactory.UpdateProject(form));
        var project = ProjectFactory.CreateProject(entity);
        return project ?? null!;
    }

    public async Task<bool> DeleteProjectAsync(string id)
    {
        var entity = await _projectRepository.GetAsync(x => x.Id == id);

        if (entity == null)
            return false;

        var result = await _projectRepository.DeleteAsync(entity);
        return result;
    }
}
