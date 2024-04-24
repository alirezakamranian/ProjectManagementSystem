﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTaskList.Request
{
    public class UpdateTaskListRequest
    {
        [Required]
        public string TaskListId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}