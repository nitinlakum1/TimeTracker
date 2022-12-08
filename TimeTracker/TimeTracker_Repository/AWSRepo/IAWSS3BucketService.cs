using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TimeTracker_Repository.AWSRepo
{
    public interface IAWSS3BucketService
    {
        Task<string> UploadFile(IFormFile formfile, string imageUrl = "");
        Task<ListVersionsResponse> FilesList();
        Task<Stream> GetFile(string key);
        Task<bool> DeleteFile(string key);
        Task<bool> DeleteMultipleFiles(List<string> imageUrls);
    }
}
