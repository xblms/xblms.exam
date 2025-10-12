using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class PathManager
    {
        public string GetDownloadFilesUrl(params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.DownloadFiles, PageUtils.Combine(paths));
        }
        public string GetTemporaryFilesUrl(params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.TemporaryFiles, PageUtils.Combine(paths));
        }
        public string GetEditUploadFilesUrl(params string[] paths)
        {
            return GetSiteFilesUrl(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Editor, PageUtils.Combine(paths));
        }
        

        public string GetSiteFilesUrl(params string[] paths)
        {
            return GetRootUrl(DirectoryUtils.SiteFiles.DirectoryName, PageUtils.Combine(paths));
        }

        public string GetSiteFilesPath(params string[] paths)
        {
            var path = PathUtils.Combine(_settingsManager.WebRootPath, DirectoryUtils.SiteFiles.DirectoryName, PathUtils.Combine(paths));
            return path;
        }

        public string GetKnowledgesUploadFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Knowledges, PageUtils.Combine(paths));
            DirectoryUtils.CreateDirectoryIfNotExists(path);
            return path;
        }

        public string GetCourseFilesUploadPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Coursefiles, PageUtils.Combine(paths));
            DirectoryUtils.CreateDirectoryIfNotExists(path);
            return path;
        }

        public string GetCerUploadFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Cer, PageUtils.Combine(paths));
            DirectoryUtils.CreateDirectoryIfNotExists(path);
            return path;
        }
        public string GetEditUploadFilesPath(params string[] paths)
        {
            var path = GetSiteFilesPath(DirectoryUtils.SiteFiles.Upload, DirectoryUtils.SiteFiles.Editor, PageUtils.Combine(paths));
            DirectoryUtils.CreateDirectoryIfNotExists(path);
            return path;
        }
        public bool IsFileExtensionAllowed(string fileExtension)
        {
            var typeCollection = Constants.DefaultImageUploadExtensions + "," + Constants.DefaultFileUploadExtensions + "," + Constants.DefaultVideoUploadExtensions;
            return PathUtils.IsFileExtensionAllowed(typeCollection, fileExtension);
        }
        public bool IsImageExtensionAllowed(string fileExtension)
        {
            var typeCollection = Constants.DefaultImageUploadExtensions;
            return PathUtils.IsFileExtensionAllowed(typeCollection, fileExtension);
        }
        public bool IsFileSizeAllowed(long contentLength)
        {
            return contentLength <= Constants.DefaultFileUploadMaxSize * 1024;
        }
        public bool IsVideoExtensionAllowed(string fileExtension)
        {
            var typeCollection = Constants.DefaultVideoUploadExtensions;
            return PathUtils.IsFileExtensionAllowed(typeCollection, fileExtension);
        }
        public bool IsVideoSizeAllowed(long contentLength)
        {
            return contentLength <= Constants.DefaultVideoUploadMaxSize * 1024;
        }
        public bool IsImageSizeAllowed(long contentLength)
        {
            return contentLength <= Constants.DefaultImageUploadMaxSize * 1024;
        }
    }
}
