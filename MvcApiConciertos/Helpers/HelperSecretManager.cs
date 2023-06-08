namespace MvcApiConciertos.Helpers
{
    using Amazon;
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;

    public static class HelperSecretManager
    {
        public static async Task<string> GetSecret()
        {
            string secretName = "secretoconcierto";
            string region = "us-east-1";

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            GetSecretValueResponse response;

            response = await client.GetSecretValueAsync(request);


            string secret = response.SecretString;

            return secret;
        }
    }
}
