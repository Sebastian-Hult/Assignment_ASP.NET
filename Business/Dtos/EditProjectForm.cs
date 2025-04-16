namespace Business.Dtos;

public class EditProjectForm
{
    public string? ProjectImage { get; set; }
    public string ProjectName { get; set; } = null!;

    public string? ClientName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public bool Status { get; set; } = false;
}
