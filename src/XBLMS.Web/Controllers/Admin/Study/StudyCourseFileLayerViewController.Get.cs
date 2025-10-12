using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFileLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var file = await _studyCourseFilesRepository.GetAsync(request.Id);
            if (file == null) return NotFound();

            file.Url = await _pathManager.GetServerFileUrl(file.Url);

            return new GetResult
            {
                FileName = file.FileName,
                FileUrl = file.Url,
                FileType = file.FileType,
                IsVideo = FileUtils.IsPlayer(file.FileType)
            };
        }
    }
}
