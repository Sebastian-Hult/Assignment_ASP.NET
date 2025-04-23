using Business.Dtos;
using Data.Entities;

namespace WebApp.ViewModels;

public class EditProjectViewModel
{
    public EditProjectForm Form { get; set; } = new();

    public List<ClientEntity> Clients { get; set; } = new();

    public List<StatusEntity> Statuses { get; set; } = new();
}
