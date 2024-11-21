namespace XBLMS.Services
{
    partial interface IPathManager
    {
        string GetCerUploadFilesPath(params string[] paths);
        string GetTemporaryFilesUrl(params string[] paths);
        
        string GetSiteFilesPath(params string[] paths);

        string GetSiteFilesUrl(params string[] paths);

        string GetEditUploadFilesUrl(params string[] paths);
        string GetEditUploadFilesPath(params string[] paths);
        bool IsFileExtensionAllowed(string fileExtension);
        bool IsImageExtensionAllowed(string fileExtension);
        bool IsFileSizeAllowed(long contentLength);
        bool IsImageSizeAllowed(long contentLength);
        bool IsVideoSizeAllowed(long contentLength);
        bool IsVideoExtensionAllowed(string fileExtension);

    }
}
