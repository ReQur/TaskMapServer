using System;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;

namespace dotnetserver
{
    public interface IAWSService
    {
        Task Upload(IFormFile file, string username);
    }

    public class AWSService : IAWSService
    {
        private readonly ILogger<UserService> _logger;
        private AmazonS3Config configsS3;
        private AmazonS3Client s3client;
        private readonly string tmpDir = "./.tmp";

        public AWSService(ILogger<UserService> logger)
        {
            _logger = logger;
            configsS3 = new AmazonS3Config
            {
                ServiceURL = "https://s3.yandexcloud.net",
                AuthenticationRegion = "ru-central1"
            };
            s3client = new AmazonS3Client(configsS3);
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
        }

        public async Task Upload(IFormFile file, string username)
        {
            string path = tmpDir + file.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var request = new PutObjectRequest
            {
                BucketName = "tm-bucket",
                Key = username + "-avatar",
                FilePath = path,
            };

            var response = await s3client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully uploaded {file.FileName}.");
            }
            else
            {
                _logger.LogInformation($"Could not upload {file.FileName}.");
            }
            File.Delete(path);
        }

    }

}