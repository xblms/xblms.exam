using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamTmDeleteLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ItemResult<ExamTm>>> Get([FromQuery] IdRequest request)
        {
            var statLog = await _statLogRepository.GetAsync(request.Id);

            var tm = TranslateUtils.JsonDeserialize<ExamTm>(statLog.LastEntity);
            if (tm != null)
            {
                await _examManager.GetTmDeleteInfo(tm);
                tm.Set("DeleteDate", statLog.CreatedDate.Value.ToString(DateUtils.FormatStringDateOnlyCN));
            }
            else
            {
                tm = new ExamTm()
                {
                    Title = "日志也被删除了"
                };
            }
     
            return new ItemResult<ExamTm>
            {
                Item = tm
            };
        }



    }
}
