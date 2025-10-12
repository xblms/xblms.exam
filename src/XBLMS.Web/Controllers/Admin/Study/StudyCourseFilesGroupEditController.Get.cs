using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesGroupEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ItemResult<StudyCourseFilesGroup>>> Get([FromQuery] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            var group = new StudyCourseFilesGroup();

            if (request.Id > 0)
            {
                group = await _studyCourseFilesGroupRepository.GetAsync(request.Id);
            }

            return new ItemResult<StudyCourseFilesGroup>
            {
                Item = group,
            };
        }
    }
}
