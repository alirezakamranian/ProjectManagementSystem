﻿using Domain.Models.ServiceResponses.ProjectTaskList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTaskList.Response
{
    public class CreateTaskListResponse
    {
        public string Message { get; set; }
        public MinimumValueTaskListDto TaskList { get; set; }
    }
}
