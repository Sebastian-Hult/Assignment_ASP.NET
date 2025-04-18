﻿namespace Business.Dtos;

public class AddProjectForm
{
    public string? ProjectImage { get; set; }
    public string ProjectName { get; set; } = null!;
    
    public string? ClientName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }

    public string ClientId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public int StatusId { get; set; }
}
