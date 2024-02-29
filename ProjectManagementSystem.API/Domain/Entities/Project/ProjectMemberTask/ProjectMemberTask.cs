using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectMemberTask
{
    public class ProjectMemberTask
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(700)]
        public string Description { get; set; }

        [Required]
        public string Order { get; set; }


        //MemberTaskListRel
        public int MemberTaskListId { get; set; }

        [ForeignKey(nameof(MemberTaskListId))]
        public ProjectMemberTaskList MemberTaskList { get; set; }
    }
}
