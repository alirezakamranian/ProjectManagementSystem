using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PMS.Domain.Entities.HumanResource;

namespace PMS.Domain.Entities.project;

public class ProjectManagement
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    [Required]
    public string ManagementRole { get; set; }


    public int EmployeeId { get; set; }
    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }

    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }
}