using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmSelectController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteRemove)]
        public async Task<ActionResult<BoolResult>> Remove([FromBody] GetSeletRemoveRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);
            var ids = group.TmIds.Where(x => !request.Ids.Contains(x)).ToList();

            group.TmIds = ids.Distinct().OrderBy(x => x).ToList().ToList();
            await _examTmGroupRepository.UpdateAsync(group);
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
