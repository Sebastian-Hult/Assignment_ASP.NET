using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class EditProjectForm
{
    public string Id { get; set; } = null!;

    public string? ProjectImage { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Project name is required")]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "Client", Prompt = "Choose a client")]
    public string? ClientName { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Description", Prompt = "Enter a description")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Start date is required")]
    [Display(Name = "Start Date", Prompt = "Enter start date")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "End Date", Prompt = "Enter end date")]
    public DateTime? EndDate { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Budget", Prompt = "Enter budget")]
    public decimal? Budget { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Status", Prompt = "Choose a status")]
    public string? Status { get; set; }

    [Required]
    public string ClientId { get; set; } = null!;


    public string? UserId { get; set; }

    [Required]
    public int StatusId { get; set; }
}
