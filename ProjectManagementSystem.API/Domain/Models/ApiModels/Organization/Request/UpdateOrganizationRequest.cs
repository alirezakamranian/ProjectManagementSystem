﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.Organization.Request
{
    public class UpdateOrganizationRequest
    {
        [Required]
        public string OrganizationId { get; set; }
        [Required]
        public string NewName { get; set; }
        public string NewDescription { get; set; }
    }
}
