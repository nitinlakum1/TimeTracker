using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using TimeTracker_Model;

namespace TimeTracker_Repository.AWSRepo
{
    public class AWSS3BucketService : IAWSS3BucketService
    {
        #region Declaration
        private readonly IAmazonS3 _amazonS3;
        private readonly AwsConfiguration _awsConfiguration;
        #endregion

        #region Constructor
        public AWSS3BucketService(AwsConfiguration awsConfiguration)
        {
            _awsConfiguration = awsConfiguration;
            _amazonS3 = new AmazonS3Client(_awsConfiguration.AwsAccessKey,
                                           _awsConfiguration.AwsSecretAccessKey,
                                           RegionEndpoint.GetBySystemName(_awsConfiguration.Region));
        }
        #endregion

        #region Methods

        public async Task<string> UploadFile(IFormFile formfile, string imageUrl = "")
        {
            try
            {
                string filePath = imageUrl;
                if (!string.IsNullOrWhiteSpace(filePath)
                    && formfile != null)
                {
                    await DeleteFile(filePath);
                    filePath = "";
                }

                if (formfile != null)
                {
                    string fileName = string.Format("{0}{1}", Common.GenerateFileName(), Path.GetExtension(formfile.FileName));

                    PutObjectRequest request = new()
                    {
                        BucketName = _awsConfiguration.BucketName,
                        Key = fileName,
                        InputStream = formfile.OpenReadStream(),
                        CannedACL = S3CannedACL.PublicRead
                    };

                    PutObjectResponse response = await _amazonS3.PutObjectAsync(request);

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    { filePath = string.Format("{0}/{1}", _awsConfiguration.RootPath, fileName); }
                }
                return filePath;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ListVersionsResponse> FilesList()
        {
            return await _amazonS3.ListVersionsAsync(_awsConfiguration.BucketName);
        }

        public async Task<Stream> GetFile(string key)
        {
            key = Path.GetFileName(key);
            GetObjectResponse response = await _amazonS3.GetObjectAsync(_awsConfiguration.BucketName, key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                bool result = false;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    key = Path.GetFileName(key);
                    DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_awsConfiguration.BucketName, key);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    { result = true; }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteMultipleFiles(List<string> imageUrls)
        {
            try
            {
                bool result = false;
                if (imageUrls != null && imageUrls.Any())
                {
                    foreach (var item in imageUrls)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            string key = Path.GetFileName(item);
                            DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_awsConfiguration.BucketName, key);

                            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                            { result = true; }
                        }
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
