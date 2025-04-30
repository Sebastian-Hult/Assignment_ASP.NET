using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectEntity CreateProject(AddProjectForm form) => new()
    {
        ProjectName = form.ProjectName,
        Image = form.ProjectImage,
        Description = form.Description,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        Budget = form.Budget,
        ClientId = form.ClientId,
        UserId = form.UserId,
        StatusId = form.StatusId,
    };

    public static Project CreateProject(ProjectEntity entity) => new()
    {
        Id = entity.Id,
        ProjectName = entity.ProjectName,
        Image = entity.Image,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Budget = entity.Budget,
        Created = entity.Created,
        ClientId = entity.ClientId,
        ClientName = entity.Client.ClientName,
        UserId = entity.UserId,
        UserFirstName = entity.User.FirstName,
        UserLastName = entity.User.LastName,
        StatusId = entity.StatusId,
        Status = entity.Status.StatusName
    };

    public static ProjectEntity UpdateProject(EditProjectForm form) => new()
    {
        ProjectName = form.ProjectName,
        Image = form.ProjectImage,
        Description = form.Description,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        Budget = form.Budget,
        ClientId = form.ClientId,
        UserId = form.UserId,
        StatusId = form.StatusId,
    };
}
