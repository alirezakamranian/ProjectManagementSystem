using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace PMS.Domain.Entities.project;

public class ProjectTask
{
    [Key]
    public int Id { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public DateTime DeadLine { get; set; }

    
    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }



    public List<ProjectTaskExecutiveAgent> ProjectTaskExecutiveAgents { get; set; }
}