using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IPathManager
    {
        Task<string> GetServerFileUrl(string url);
        string ContentRootPath { get; }

        string WebRootPath { get; }

        string GetContentRootPath(params string[] paths);

        string GetAdminUrl(params string[] paths);

        string GetHomeUrl(params string[] paths);

        string GetUploadFileName(string fileName);

        string ParsePath(string virtualPath);

        Task UploadAsync(IFormFile file, string filePath);

        Task UploadAsync(byte[] bytes, string filePath);
    }
}
