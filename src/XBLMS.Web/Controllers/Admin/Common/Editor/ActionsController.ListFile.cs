using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.Editor
{
    public partial class ActionsController
    {
        [HttpGet, Route(RouteActionsListFile)]
        public ActionResult<ListFileResult> ListFile([FromQuery] ListFileRequest request)
        {

            var directoryPath = _pathManager.GetEditUploadFilesPath();


            var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories).Where(x =>
                _pathManager.IsFileExtensionAllowed(PathUtils.GetExtension(x))).OrderByDescending(x => x);

            var list = new List<FileResult>();
            foreach (var x in files.Skip(request.Start).Take(request.Size))
            {
                list.Add(new FileResult
                {
                    Url = _pathManager.GetRootUrlByPath(x)
                });
            }

            return new ListFileResult
            {
                State = "SUCCESS",
                Size = request.Size,
                Start = request.Start,
                Total = files.Count(),
                List = list
            };
        }
    }
}
