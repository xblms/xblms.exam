using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IPathManager
    {
        string DefaultBookCoverUrl { get; }
        string DefaultAvatarUrl { get; }
        string DefaultCourseCoverUrl { get; }
        string DefaultCourseFaceCoverUrl { get; }
        string DefaultCoursePlanCoverUrl { get; }

        string GetRootUrl(params string[] paths);

        string GetRootUrlByPath(string physicalPath);

        string GetRootPath(params string[] paths);

        bool IsInRootDirectory(string filePath);

        string GetTemporaryFilesPath(params string[] paths);
        string GetDownloadFilesPath(params string[] paths);
        string GetDownloadFilesUrl(params string[] paths);
        string GetImportFilesPath(params string[] paths);

        Task<string> WriteTemporaryTextAsync(string value);

        string ParseUrl(string url);

        string ParsePath(string directoryPath, string virtualPath);

        string GetAdministratorUploadPath(string userName, params string[] paths);

        string GetAdministratorUploadUrl(string userName, params string[] paths);
        string GetAdminAvatarUploadPath(string userName, params string[] paths);

        string GetAdminAvatarUploadUrl(string userName, params string[] paths);

        string GetUserUploadPath(string userName, params string[] paths);

        string GetUserUploadUrl(string userName, params string[] paths);
        string GetUserAvatarUploadPath(string userName, params string[] paths);

        string GetUserAvatarUploadUrl(string userName, params string[] paths);

        string GetHomeUploadPath(params string[] paths);

        string GetHomeUploadUrl(params string[] paths);

        string GetUserUploadPath(int userId, string relatedPath);

        string GetUserUploadFileName(string filePath);

        string GetUserUploadUrl(int userId, string relatedUrl);

        string GetUserAvatarUrl(User user);
        string GetCoverUploadPath(params string[] paths);
        string GetCoverUploadUrl(params string[] paths);

    }
}
