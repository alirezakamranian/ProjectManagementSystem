using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PMS.Domain.Entities.project;

public class Project
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required] 
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime DeadLine { get; set; }

    public string Status { get; set; }


    public List<ProjectTask> ProjectTasks { get; set; }

    public ProjectManagement ProjectManagement { get; set; }
}