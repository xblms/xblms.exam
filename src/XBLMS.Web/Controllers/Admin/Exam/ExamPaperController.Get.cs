using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _examPaperRepository.GetListAsync(adminAuth, request.TreeIsChildren, request.TreeId, request.Keyword, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var markCount = await _examPaperStartRepository.CountByMarkAsync(item.Id);
                    item.Set("MarkCount", markCount);

                    var creatorAdmin = await _adminRepository.GetByUserIdAsync(item.CreatorId);
                    if (creatorAdmin != null)
                    {
                        item.Set("Creator", creatorAdmin.DisplayName);
                    }
                    else
                    {
                        item.CreatorId = 0;
                        item.Set("Creator", "已删除");
                    }

                    var useCount = await _studyManager.GetUseCountByPaperId(item.Id);
                    item.Set("UseCount", useCount);
                }
            }
            return new GetResult
            {
                Total = total,
                Items = list,
            };
        }

    }
}
