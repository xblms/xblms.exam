using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class EditorTmWordOpenLayerController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSubmit)]
        public async Task<IntResult> Submit([FromBody] GetRequest reqeust)
        {
            var admin = await _authManager.GetAdminAsync();

            var (total, successTotal, errorTotal, tmList, resultHtml) = await Check(reqeust.TmHtml, reqeust.TreeId, admin);
            successTotal = 0;
            if (tmList!=null && tmList.Count>0)
            {
                foreach (var tm in tmList)
                {
                    await _examTmRepository.InsertAsync(tm);
                    successTotal++;

                    await _statRepository.AddCountAsync(StatType.ExamTmAdd);
                    await _authManager.AddAdminLogAsync("新增题目", $"{ StringUtils.StripTags(tm.Title)}");
                }
            }

            return new IntResult
            {
                Value = successTotal
            };
        }
    }
}
