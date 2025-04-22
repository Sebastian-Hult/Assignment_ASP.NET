using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }
    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;


    public string ClientId { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string UserFirstName { get; set; } = null!;
    public string UserLastName { get; set; } = null!;
    public int StatusId { get; set; }
    public string Status { get; set; } = null!;
}
