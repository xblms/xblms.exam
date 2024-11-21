using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmSelectController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSelect)]
        public async Task<ActionResult<BoolResult>> Select([FromBody] GetSeletRemoveRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);
            if (group.TmIds != null)
            {
                group.TmIds.AddRange(request.Ids);
            }
            else
            {
                group.TmIds = request.Ids;
            }
            group.TmIds = group.TmIds.ToList().Distinct().OrderBy(x => x).ToList().ToList();
            await _examTmGroupRepository.UpdateAsync(group);
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
