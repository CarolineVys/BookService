namespace BookService.Services
{
    public interface IAwsService
    {
        Task<string> UploadPdfToS3Async(MemoryStream fileStream, string fileName, string contentType);
        Task SendMessageToAuditQueueAsync(string messageBody);
    }
}
