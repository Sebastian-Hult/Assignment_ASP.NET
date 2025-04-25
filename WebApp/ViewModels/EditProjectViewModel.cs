using Business.Dtos;
using Data.Entities;

namespace WebApp.ViewModels;

public class EditProjectViewModel
{
    public EditProjectForm Form { get; set; } = new();

    public IEnumerable<ClientEntity> Clients { get; set; } = [];

    public IEnumerable<StatusEntity> Statuses { get; set; } = [];
}
