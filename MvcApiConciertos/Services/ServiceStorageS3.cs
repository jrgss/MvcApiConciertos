using Amazon.S3;
using Amazon.S3.Model;
using MvcApiConciertos.Helpers;
using MvcApiConciertos.Models;
using Newtonsoft.Json;

namespace MvcApiConciertos.Services
{
    public class ServiceStorageS3
    {
        private IAmazonS3 ClientS3;
        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clients3)
        {
            this.ClientS3 = clients3;
        }

        public async Task<bool> UploadFileAsync(string fileName, Stream stream)
        {
            string jsonSecreto = await HelperSecretManager.GetSecret();
            KeysModel modelo = JsonConvert.DeserializeObject<KeysModel>(jsonSecreto);
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = modelo.BucketName,
            };
            PutObjectResponse response = await ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
