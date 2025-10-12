﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentConfigController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _examAssessmentConfigRepository.GetListAsync(adminAuth, request.Keyword, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("PaperCount", await _examAssessmentRepository.GetConfigCount(item.Id));
                    item.Set("TotalItem", await _examAssessmentConfigSetRepository.GetCountAsync(item.Id));
                }
            }

            return new GetResult
            {
                Total = total,
                List = list,
            };
        }

    }
}
