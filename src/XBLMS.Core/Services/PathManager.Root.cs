using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class PathManager
    {
        public string GetRootUrl(params string[] paths)
        {
            return PageUtils.Combine("/", PageUtils.Combine(paths));
        }

        public string GetRootUrlByPath(string physicalPath)
        {
            var requestPath = PathUtils.GetPathDifference(_settingsManager.WebRootPath, physicalPath);
            requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
            return GetRootUrl(requestPath);
        }

        public string ParseUrl(string virtualUrl)
        {
            if (string.IsNullOrEmpty(virtualUrl)) return string.Empty;
            if (PageUtils.IsAbsoluteUrl(virtualUrl)) return virtualUrl;

            virtualUrl = virtualUrl.StartsWith("~") ? GetRootUrl(virtualUrl.Substring(1)) : virtualUrl;
            virtualUrl = virtualUrl.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
            virtualUrl = virtualUrl.Replace(PageUtils.DoubleSeparator, PageUtils.Separator);
            return virtualUrl;
        }

        public string ParsePath(string virtualPath)
        {
            virtualPath = PathUtils.RemovePathInvalidChar(virtualPath);
            if (!string.IsNullOrEmpty(virtualPath))
            {
                if (virtualPath.StartsWith("~"))
                {
                    virtualPath = virtualPath.Substring(1);
                }
                virtualPath = PageUtils.Combine("~", virtualPath);
            }
            else
            {
                virtualPath = "~/";
            }
            var rootPath = WebRootPath;

            virtualPath = !string.IsNullOrEmpty(virtualPath) ? virtualPath.Substring(2) : string.Empty;
            var retVal = PathUtils.Combine(rootPath, virtualPath) ?? string.Empty;

            return retVal.Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar);
        }

        public string ParsePath(string directoryPath, string virtualPath)
        {
            var resolvedPath = virtualPath;
            if (string.IsNullOrEmpty(virtualPath))
            {
                virtualPath = "@";
            }
            if (!virtualPath.StartsWith("@") && !virtualPath.StartsWith("~"))
            {
                virtualPath = "@" + virtualPath;
            }
            if (virtualPath.StartsWith("@"))
            {
                if (string.IsNullOrEmpty(directoryPath))
                {
                    resolvedPath = string.Concat("~", virtualPath.Substring(1));
                }
                else
                {
                    return PageUtils.Combine(directoryPath, virtualPath.Substring(1));
                }
            }
            return ParsePath(resolvedPath);
        }





        public string GetHomeUploadUrl(params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Home, PageUtils.Combine(paths));
        }

        public string DefaultAvatarUrl => GetSiteFilesUrl("assets/images/default_avatar.png");

        public string GetUserUploadFileName(string filePath)
        {
            var dt = DateTime.Now;
            return $"{dt.Day}{dt.Hour}{dt.Minute}{dt.Second}{dt.Millisecond}{PathUtils.GetExtension(filePath)}";
        }

        public string GetUserUploadUrl(int userId, string relatedUrl)
        {
            return GetHomeUploadUrl(userId.ToString(), relatedUrl);
        }
        public string GetUserUploadUrl(string userName, params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Users,
                PageUtils.Combine(userName, PageUtils.Combine(paths)));
        }
        public string GetUserUploadPath(string userName, params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Users, PathUtils.Combine(userName, PathUtils.Combine(paths)));
            return path;
        }
        public string GetUserAvatarUploadUrl(string userName, params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Avatar, DirectoryUtils.SiteFiles.Users,
                PageUtils.Combine(userName, PageUtils.Combine(paths)));
        }
        public string GetUserAvatarUploadPath(string userName, params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Avatar, DirectoryUtils.SiteFiles.Users, PathUtils.Combine(userName, PathUtils.Combine(paths)));
            return path;
        }
        public string GetUserAvatarUrl(User user)
        {
            var imageUrl = user?.AvatarUrl;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                return PageUtils.IsAbsoluteUrl(imageUrl) ? imageUrl : GetUserUploadUrl(user.Id, imageUrl);
            }

            return DefaultAvatarUrl;
        }

        public string GetRootPath(params string[] paths)
        {
            var path = PathUtils.Combine(_settingsManager.WebRootPath, PathUtils.Combine(paths));
            if (DirectoryUtils.IsInDirectory(_settingsManager.WebRootPath, path))
            {
                return path;
            }
            return _settingsManager.WebRootPath;
        }

        public bool IsInRootDirectory(string filePath)
        {
            return DirectoryUtils.IsInDirectory(_settingsManager.WebRootPath, filePath);
        }

        public string GetContentRootPath(params string[] paths)
        {
            var path = PathUtils.Combine(_settingsManager.ContentRootPath, PathUtils.Combine(paths));
            if (DirectoryUtils.IsInDirectory(_settingsManager.ContentRootPath, path))
            {
                return path;
            }
            return _settingsManager.ContentRootPath;
        }
        public string GetAdministratorUploadUrl(string userName, params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Administrators,
                PageUtils.Combine(userName, PageUtils.Combine(paths)));
        }
        public string GetAdministratorUploadPath(string userName, params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Administrators, PathUtils.Combine(userName, PathUtils.Combine(paths)));
            return path;
        }
        public string GetAdminAvatarUploadUrl(string userName, params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Avatar, DirectoryUtils.SiteFiles.Administrators,
                PageUtils.Combine(userName, PageUtils.Combine(paths)));
        }
        public string GetAdminAvatarUploadPath(string userName, params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Avatar, DirectoryUtils.SiteFiles.Administrators, PathUtils.Combine(userName, PathUtils.Combine(paths)));
            return path;
        }
        public string GetImportFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.ImportFiles, PathUtils.Combine(paths));
            return path;
        }

        public string GetHomeUploadPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Home, PathUtils.Combine(paths));
            return path;
        }

        public string GetTemporaryFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.TemporaryFiles, PathUtils.Combine(paths));
            return path;
        }
        public string GetDownloadFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.DownloadFiles, PathUtils.Combine(paths));
            return path;
        }
        public async Task<string> WriteTemporaryTextAsync(string value)
        {
            var fileName = $"{StringUtils.Guid()}.json";
            var jsonFilePath = GetTemporaryFilesPath(fileName);
            FileUtils.DeleteFileIfExists(jsonFilePath);
            await FileUtils.WriteTextAsync(jsonFilePath, value);
            return fileName;
        }

        public string GetUserUploadPath(int userId, string relatedPath)
        {
            return GetHomeUploadPath(userId.ToString(), relatedPath);
        }

    }
}
