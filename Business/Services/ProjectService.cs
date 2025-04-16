using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectEntity> CreateProjectAsync();
    Task<IEnumerable<ProjectEntity>> GetProjectAsync(string id);
    Task<IEnumerable<ProjectEntity>> GetProjectsAsync();
}

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<ProjectEntity> CreateProjectAsync()
    {
        var project = new ProjectEntity
        {
            ProjectName = "New Project",
            Description = "Project Description",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1),
            Budget = 10000,
            ClientId = "ClientId",
            UserId = "UserId",
            StatusId = 1
        };
        await _projectRepository.AddAsync(project);
        //return await _projectRepository.GetAllAsync();
    }

    public async Task<IEnumerable<ProjectEntity>> GetProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects;
    }

    public async Task<IEnumerable<ProjectEntity>> GetProjectAsync(string id)
    {
        var project = await _projectRepository.GetAsync(x => x.Id == id);
        // factory

        return project;
    }
}
