using Domain.Models.InternalSerives.Storage.Request;
using Domain.Models.ServiceResponses.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IStorageService
    {
        public Task<UploadFileServiceResponse> Upload(UploadFileRequest request,string userId);
        public Task<GetFileUrlServiceResponse> GetUrl(GetFileUrlRequest request);
    }
}
