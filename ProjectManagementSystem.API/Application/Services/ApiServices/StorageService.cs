using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Domain.Models.InternalSerives.Storage.Request;
using Domain.Models.ServiceResponses.Storage;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.ApiServices
{
    public class StorageService(IConfiguration configuration,
       ILogger<StorageService> logger,
       DataContext context) : IStorageService
    {
        private static IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<StorageService> _logger = logger;
        private readonly DataContext _context = context;
        /// <summary>
        /// Uploads file to storage
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>UploadFileServiceResponse</returns>
        public async Task<UploadFileServiceResponse> Upload(UploadFileRequest request, string userId)
        {
            try
            {
                var awsCredentials = new Amazon.Runtime
                .BasicAWSCredentials(
                    _configuration["AwsS3:AccessKey"],
                    _configuration["AwsS3:SecretKey"]);

                var config = new AmazonS3Config
                { ServiceURL = _configuration["AwsS3:UrlAdress"] };

                _s3Client = new AmazonS3Client
                    (awsCredentials, config);

                using (MemoryStream stream = new())
                {
                    request.File.CopyTo(stream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = stream,
                        Key = request.Key,
                        BucketName = _configuration["AwsS3:BucketName"],
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(_s3Client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                _context.StorageItemsUrls.Add(new()
                {
                    TargetEntityId = request.Key.ToString(),
                    Url = $"https://projectmanagementsystem.s3.ir-thr-at1.arvanstorage.ir/{request.Key}"
                });

                await _context.SaveChangesAsync();

                return new UploadFileServiceResponse(
                     Domain.Models.ServiceResponses.Base.ServiceResponseStatusBase.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UploadFileService : {Message}", ex.Message);

                return new UploadFileServiceResponse(
                     Domain.Models.ServiceResponses.Base.ServiceResponseStatusBase.InternalError);
            }
        }

        /// <summary>
        /// Used for getting url of file on storage
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetFileUrlServiceResponse</returns>
        public async Task<GetFileUrlServiceResponse> GetUrl(GetFileUrlRequest request)
        {
            try
            {
                var ItemFileUrl = await _context.StorageItemsUrls
                    .FirstOrDefaultAsync(s => s.TargetEntityId
                        .Equals(request.FileKey));

                if (ItemFileUrl == null)
                    return new GetFileUrlServiceResponse(
                         GetFileUrlServiceResponseStatus.Success)
                    {
                        Url = null
                    };


                return new GetFileUrlServiceResponse(
                     GetFileUrlServiceResponseStatus.Success)
                {
                    Url = ItemFileUrl.Url
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetFileUrlService : {Message}", ex.Message);

                return new GetFileUrlServiceResponse(
                     GetFileUrlServiceResponseStatus.InternalError);
            }
        }
    }
}
