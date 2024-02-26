using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PMS.Domain.Entities.HumanResource;
using PMS.Domain.Entities.HumanResource.team;

namespace PMS.Domain.Entities.project;

public class ProjectTaskExecutiveAgent
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    [Required]
    public string Role { get; set; }


    public int EmployeeId { get; set; }
    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }

    public int TeamId { get; set; }
    [ForeignKey("TeamId")]
    public Team Team { get; set; }


    public int ProjectTaskId { get; set; }
    [ForeignKey("ProjectTaskId")]
    public ProjectTask ProjectTask { get; set; }
}