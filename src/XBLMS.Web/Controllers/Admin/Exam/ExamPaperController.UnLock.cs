using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperController
    {
        [HttpPost, Route(RouteUnLock)]
        public async Task<ActionResult<BoolResult>> UnLocked([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var paper = await _examPaperRepository.GetAsync(request.Id);
            if (paper == null) return NotFound();
            paper.Locked = false;

            await _examPaperRepository.UpdateAsync(paper);
            await _examPaperUserRepository.UpdateLockedAsync(paper.Id, paper.Locked);
            await _examPaperStartRepository.UpdateLockedAsync(paper.Id, paper.Locked);

            await _authManager.AddAdminLogAsync("解锁试卷", $"试卷：{paper.Title}");

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
