using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var (total, successTotal, errorTotal, tmList, resultHtml) = await Check(reqeust.TmHtml, reqeust.TreeId, admin, adminAuth);
            successTotal = 0;

            var treePath = new List<string>();
            if (reqeust.TreeId > 0)
            {
                var tree = await _examTmTreeRepository.GetAsync(reqeust.TreeId);
                if (tree != null)
                {
                    treePath = tree.ParentPath;
                }
            }

            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tm.TreeParentPath = treePath;
                    var tmId = await _examTmRepository.InsertAsync(tm);
                    successTotal++;

                    await _authManager.AddAdminLogAsync("新增题目", $"{StringUtils.StripTags(tm.Title)}");
                    await _authManager.AddStatLogAsync(StatType.ExamTmAdd, "新增题目", tmId, StringUtils.StripTags(tm.Title));
                    await _authManager.AddStatCount(StatType.ExamTmAdd);
                }
            }

            return new IntResult
            {
                Value = successTotal
            };
        }
    }
}
