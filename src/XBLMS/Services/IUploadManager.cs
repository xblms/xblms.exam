using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public interface IUploadManager
    {
        Task<(bool success, string msg, string path)> UploadAvatar(IFormFile file, UploadManageType uploadType, string userName);
    }

}
