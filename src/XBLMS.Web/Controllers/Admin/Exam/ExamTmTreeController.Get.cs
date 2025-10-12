using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var trees = await _examManager.GetExamTmTreeCascadesAsync(adminAuth);
            var tmGroups = await _examTmGroupRepository.GetListAsync(adminAuth, string.Empty, true);
            var txList = await _examTxRepository.GetListAsync();
            var orderTypeList = ListUtils.GetSelects<OrderType>();

            return new GetResult
            {
                Items = trees,
                TxList = txList,
                OrderTypeList = orderTypeList,
                TmGroups = tmGroups
            };
        }

        [HttpGet, Route(RouteTmTotal)]
        public async Task<ActionResult<GetTmTotalResult>> GetTmTotal([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var (count, total) = await _examTmRepository.GetTotalAndCountByTreeIdAsync(adminAuth, request.Id);

            return new GetTmTotalResult
            {
                Total = total,
                Count = count
            };
        }
    }
}
