using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public class UploadManager : IUploadManager
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IPathManager _pathManager;
        private readonly IAuthManager _authManager;


        public UploadManager(ISettingsManager settingsManager, IPathManager pathManager, IAuthManager authManager)
        {
            _settingsManager = settingsManager;
            _pathManager = pathManager;
            _authManager = authManager;
        }
        public async Task<(bool success, string msg, string path)> UploadAvatar(IFormFile file, UploadManageType uploadType, string userName)
        {
            if (file == null) return (false, Constants.ErrorUpload, "");

            var fileName = _pathManager.GetUploadFileName(file.FileName);

            var returnUrl = "";

            if (uploadType == UploadManageType.AdminAvatar)
            {
                var filePath = _pathManager.GetAdminAvatarUploadPath(userName, fileName);
                if (!FileUtils.IsImage(PathUtils.GetExtension(fileName)))
                {
                    return (false, Constants.ErrorImageExtensionAllowed, "");
                }

                await _pathManager.UploadAsync(file, filePath);
                returnUrl = _pathManager.GetAdminAvatarUploadUrl(userName, fileName);
            }
            else if (uploadType == UploadManageType.UserAvatar)
            {
                var filePath = _pathManager.GetUserAvatarUploadPath(userName, fileName);
                if (!FileUtils.IsImage(PathUtils.GetExtension(fileName)))
                {
                    return (false, Constants.ErrorImageExtensionAllowed, "");
                }
                await _pathManager.UploadAsync(file, filePath);
                returnUrl = _pathManager.GetUserAvatarUploadUrl(userName, fileName);
            }


            return (true, "", returnUrl);
        }
    }
}
