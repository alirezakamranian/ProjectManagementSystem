using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.InternalSerives.Storage.Request;
using Domain.Models.InternalSerives.Storage.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.Storage;
using Domain.Services.InternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.Storage
{
    [Route("storage")]
    [ApiController]
    public class StorageController(IStorageService storageService) : ControllerBase
    {

        private readonly IStorageService _storageService = storageService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadFileRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _storageService
                .Upload(request, userId);

            if (serviceResponse.Status.Equals(UploadFileServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new UploadFileResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new UploadFileResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
