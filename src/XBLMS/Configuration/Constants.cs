using System;

namespace XBLMS.Configuration
{
    public static class Constants
    {
        public const string AdminTitle = "XBLMS";
        public const string EnvironmentPrefix = "XBLMS_";
        public const string ConfigFileName = "xblms.json";
        public const string PackageFileName = "package.json";
        public const string ReadmeFileName = "README.md";
        public const string AdminDirectory = "xblms-admin";
        public const string HomeDirectory = "home";
        public const string WwwrootDirectory = "wwwroot";
        public const string DefaultLanguage = "en";
        public const string EncryptStingIndicator = "0secret0";

        public const string ApiPrefix = "/api";
        public const string ApiAdminPrefix = "/api/admin";
        public const string ApiHomePrefix = "/api/home";
        public const string ApiV1Prefix = "/api/v1";

        public const string ScopeAdministrators = "Administrators";
        public const string ScopeUsers = "Users";
        public const string ScopeOthers = "Others";
        public const string OfficialHost = "https://xblms.com";
        public const string SessionIdPrefix = "SESSION-ID-";

        public const int AccessTokenExpireDays = 7;
        public static DateTime SqlMinValue { get; } = new DateTime(1754, 1, 1, 0, 0, 0, 0);

        public static string GetSessionIdCacheKey(int userId)
        {
            return $"{SessionIdPrefix}{userId}";
        }
        public static string GetUserSessionIdCacheKey(int userId)
        {
            return $"{SessionIdPrefix}USER-{userId}";
        }

        public const char Newline = '\n';//换行
        public const string ReturnAndNewline = "\r\n";//回车换行
        public const string Html5Empty = @"<html>
  <head>
    <meta charset=""utf-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"">
    <meta name=""renderer"" content=""webkit"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
    <meta http-equiv=""cache-control"" content=""max-age=0"" />
    <meta http-equiv=""cache-control"" content=""no-cache"" />
    <meta http-equiv=""expires"" content=""0"" />
    <meta http-equiv=""expires"" content=""Tue, 01 Jan 1980 1:00:00 GMT"" />
    <meta http-equiv=""pragma"" content=""no-cache"" />
  </head>
  <body><script>location.href=""/xblms-admin/""</script></body>
</html>
";

        public const string Ellipsis = "...";

        public const int PageSize = 25;//后台分页数
        public const string SmallImageAppendix = "s_";


        public const string ActionsLoginSuccess = "登录成功";
        public const string ActionsLoginFailure = "登录失败";

        public const string ErrorSafe = "系统已开启安全模式，该功能被禁止使用!";

        public const string ErrorNotFound = "未找到该数据!";
        public const string ErrorUpload = "请选择有效的文件上传!";
        public const string ErrorImageExtensionAllowed = "此图片格式已被禁止上传!";
        public const string ErrorImageSizeAllowed = "此图片大小已超过限制，请压缩后上传!";
        public const string ErrorVideoExtensionAllowed = "此视频格式已被禁止上传!";
        public const string ErrorVideoSizeAllowed = "此视频大小已超过限制，请压缩后上传!";
        public const string ErrorAudioExtensionAllowed = "此音频格式已被禁止上传!";
        public const string ErrorAudioSizeAllowed = "此音频大小已超过限制，请压缩后上传!";
        public const string ErrorFileExtensionAllowed = "此文件格式已被禁止上传!";
        public const string ErrorFileSizeAllowed = "此文件大小已超过限制，请压缩后上传!";

        public const long DefaultImageUploadMaxSize = 1024 * 1024 * 1024;//M
        public const string DefaultImageUploadExtensions = ".gif,.jpg,.jpeg,.bmp,.png,.pneg,.swf,.webp";

        public const long DefaultAudioUploadMaxSize = 1024 * 1024 * 1024;//M
        public const string DefaultAudioUploadExtensions = ".mp3,.wma,.wav,.amr,.m4a";

        public const string DefaultVideoUploadExtensions = ".asf,.asx,.avi,.flv,.mid,.midi,.mov,.mp3,.mp4,.mpg,.mpeg,.ogg,.ra,.rm,.rmb,.rmvb,.rp,.rt,.smi,.swf,.wav,.webm,.wma,.wmv,.viv,.f4v,.m4v,.3gp,.3g2";
        public const long DefaultVideoUploadMaxSize = 1024 * 1024 * 1024;//M

        public const string DefaultFileUploadExtensions = ".zip,.rar,.7z,.tar,.gz,.bz2,.cab,.iso,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.pdf,.md,.xml,.css";
        public const long DefaultFileUploadMaxSize = 1024 * 1024 * 1024;//M

        public const string DefaultFileDownloadExtensions = ".zip,.rar,.7z,.tar,.gz,.bz2,.cab,.iso,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.pdf,.mp3,.mp4,.webm,.apk";
    }
}
