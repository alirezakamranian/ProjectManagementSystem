using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InternalSerives.Storage.Request
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
        public string Key { get; set; }
    }
}
