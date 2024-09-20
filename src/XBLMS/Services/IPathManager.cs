using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IPathManager
    {
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
