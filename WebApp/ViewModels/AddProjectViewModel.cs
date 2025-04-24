using Business.Dtos;
using Data.Entities;

namespace WebApp.ViewModels;

public class AddProjectViewModel
{
    public AddProjectForm Form { get; set; } = new();

    public IEnumerable<ClientEntity> Clients { get; set; } = [];

    public IEnumerable<StatusEntity> Statuses { get; set; } = [];
}
