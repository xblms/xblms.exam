using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class PathManager : IPathManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;

        public PathManager(ICacheManager cacheManager, ISettingsManager settingsManager, IDatabaseManager databaseManager)
        {
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _databaseManager = databaseManager;
        }


        public string ContentRootPath => _settingsManager.ContentRootPath;
        public string WebRootPath => _settingsManager.WebRootPath;

        public string GetAdminUrl(params string[] paths)
        {
            return PageUtils.Combine($"/{Constants.AdminDirectory}", PageUtils.Combine(paths), "/");
        }

        public string GetHomeUrl(params string[] paths)
        {
            return PageUtils.Combine($"/{Constants.HomeDirectory}", PageUtils.Combine(paths));
        }

        //public string GetApiUrl(Site site, params string[] paths)
        //{
        //    return GetApiHostUrl(site, Constants.ApiPrefix, PageUtils.Combine(paths));
        //}

        public string GetUploadFileName(string fileName)
        {
            var dt = DateTime.Now;
            return $"{dt.Day}{dt.Hour}{dt.Minute}{dt.Second}{dt.Millisecond}{StringUtils.GetRandomInt(1, 9999)}{PathUtils.GetExtension(fileName)}";
        }

        public async Task UploadAsync(IFormFile file, string filePath)
        {
            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public async Task UploadAsync(byte[] bytes, string filePath)
        {
            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await stream.WriteAsync(bytes);
        }
    }
}
