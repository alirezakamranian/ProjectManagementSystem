using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class StorageItemUrl
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string TargetEntityId { get; set; }
    }
}
