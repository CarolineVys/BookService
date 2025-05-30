using Amazon.S3;
using Amazon.SQS;
using BookService.Configuration;
using Microsoft.Extensions.Options;
using Amazon.S3.Model;

namespace BookService.Services
{
    public class AwsService : IAwsService
    {
        private readonly IAmazonS3 _awsS3Client;
        private readonly IAmazonSQS _awsSQSClient;
        private readonly AppSettings _appSettings;

        public AwsService(IAmazonS3 amazonS3Client, IAmazonSQS amazonSQSClient, IOptions<AppSettings> settings)
        {
            _awsS3Client = amazonS3Client;
            _awsSQSClient = amazonSQSClient;
            _appSettings = settings.Value;
        }

        public async Task<string> UploadPdfToS3Async(MemoryStream fileStream, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                InputStream = fileStream,
                ContentType = contentType,
                BucketName = _appSettings.AwsBooksS3Bucket,
                Key = $"books/{fileName}"
            };

            try
            {
                var result = await _awsS3Client.PutObjectAsync(request);
            }
            catch (Exception e)
            {
                throw new Exception($"Something went wrong while uploading file {fileName} to S3. Message: {e.Message}");
            }

            return _appSettings.AwsBooksS3BucketUrl + fileName;
        }

        public async Task SendMessageToAuditQueueAsync(string messageBody)
        {
            await _awsSQSClient.SendMessageAsync(_appSettings.AwsSqsAuditUrl, messageBody);
        }
    }
}
