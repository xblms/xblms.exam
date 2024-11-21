using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class EditorTmWordOpenLayerController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSubmit)]
        public async Task<BoolResult> Submit([FromBody] GetRequest reqeust)
        {
            var admin = await _authManager.GetAdminAsync();

            var (total, successTotal, errorTotal, tmList, resultHtml) = await Check(reqeust.TmHtml, reqeust.TreeId, admin);

            if (tmList!=null && tmList.Count>0)
            {
                foreach (var tm in tmList)
                {
                    await _examTmRepository.InsertAsync(tm);

                    await _statRepository.AddCountAsync(StatType.ExamTmAdd);
                    await _authManager.AddAdminLogAsync("新增题目", $"{HtmlUtils.ClearFormat(tm.Title)}");
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
