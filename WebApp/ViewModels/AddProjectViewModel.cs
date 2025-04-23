using Business.Dtos;
using Data.Entities;

namespace WebApp.ViewModels;

public class AddProjectViewModel
{
    public AddProjectForm Form { get; set; } = new();

    public List<ClientEntity?> Clients { get; set; } = new();

    public List<StatusEntity?> Statuses { get; set; } = new();
}
