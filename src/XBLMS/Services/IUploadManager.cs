using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using XBLMS.Enums;

namespace XBLMS.Services
{
    public interface IUploadManager
    {
        Task<(bool success, string msg, string path)> UploadAvatar(IFormFile file, UploadManageType uploadType, string userName);
        Task<(bool success, string msg, string path)> UploadCover(IFormFile file);
    }

}
