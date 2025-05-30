namespace BookService.Configuration
{
    public class AppSettings
    {
        public string AwsBooksS3Bucket { get; set; }
        public string AwsBooksS3BucketUrl { get; set; }
        public string AwsSqsAuditUrl { get; set; }
    }
}
