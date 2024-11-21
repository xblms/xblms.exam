using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.Editor
{
    public partial class ActionsController
    {
        [HttpGet, Route(RouteActionsConfig)]
        public ActionResult<ConfigResult> GetConfig()
        {
            return new ConfigResult
            {
                ImageActionName = "uploadImage",
                ImageFieldName = "file",
                ImageMaxSize = Constants.DefaultImageUploadMaxSize,
                ImageAllowFiles = ListUtils.GetStringList(Constants.DefaultImageUploadExtensions),
                ImageCompressEnable = false,
                ImageCompressBorder = 1600,
                ImageInsertAlign = "none",
                ImageUrlPrefix = "",
                ImagePathFormat = "",
                ScrawlActionName = "uploadScrawl",
                ScrawlFieldName = "file",
                ScrawlPathFormat = "",
                ScrawlMaxSize = Constants.DefaultImageUploadMaxSize,
                ScrawlUrlPrefix = "",
                ScrawlInsertAlign = "none",
                VideoActionName = "uploadVideo",
                VideoFieldName = "file",
                VideoUrlPrefix = "",
                VideoMaxSize = Constants.DefaultVideoUploadMaxSize,
                VideoAllowFiles = ListUtils.GetStringList(Constants.DefaultVideoUploadExtensions),
                FileActionName = "uploadFile",
                FileFieldName = "file",
                FileUrlPrefix = "",
                FileMaxSize = Constants.DefaultFileUploadMaxSize,
                FileAllowFiles = ListUtils.GetStringList(Constants.DefaultFileUploadExtensions),
                ImageManagerActionName = "listImage",
                ImageManagerListSize = 20,
                ImageManagerUrlPrefix = "",
                ImageManagerInsertAlign = "none",
                ImageManagerAllowFiles = ListUtils.GetStringList(Constants.DefaultImageUploadExtensions),
                FileManagerActionName = "listFile",
                FileManagerListSize = 20,
                FileManagerUrlPrefix = "",
                FileManagerAllowFiles = ListUtils.GetStringList($"{Constants.DefaultImageUploadExtensions},{Constants.DefaultVideoUploadExtensions},{Constants.DefaultFileUploadExtensions}")
            };
        }
    }
}
