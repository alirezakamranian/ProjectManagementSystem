using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InternalSerives.Storage.Request
{
    public class GetFileUrlRequest
    {
        [Required]
        public string FileKey { get; set; }
    }
}
