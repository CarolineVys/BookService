using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace BookService.Configuration
{
    public static class AmazonSecretManager
    {
        public static async Task<string> GetSecretsFromAWS(string secretName)
        {
            var manager = new AmazonSecretsManagerClient();
            var request = new GetSecretValueRequest
            {
                SecretId = secretName,
            };

            try
            {
                var response = await manager.GetSecretValueAsync(request);
                if (response.SecretString != null)
                {
                    return response.SecretString;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }
    }
}
