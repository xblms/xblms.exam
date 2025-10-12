using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Analysis
{
    public partial class AnalysisAdminLoginController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var lowerDate = TranslateUtils.ToDateTime(request.DateFrom);
            var higherDate = TranslateUtils.ToDateTime(request.DateTo, DateTime.Now);

            var successStats = await _statRepository.GetStatsAsync(adminAuth, lowerDate, higherDate, StatType.AdminLoginSuccess);
            var failureStats = await _statRepository.GetStatsAsync(adminAuth, lowerDate, higherDate, StatType.UserLogin);

            var getStats = new List<GetStat>();
            var totalDays = (higherDate - lowerDate).TotalDays;
            for (var i = 0; i <= totalDays; i++)
            {
                var date = lowerDate.AddDays(i).ToString("M-d");

                var success = successStats.Where(x => x.CreatedDate.HasValue && x.CreatedDate.Value.ToString("M-d") == date);
                var failure = failureStats.Where(x => x.CreatedDate.HasValue && x.CreatedDate.Value.ToString("M-d") == date);

                getStats.Add(new GetStat
                {
                    Date = date,
                    Success = success.Sum(x => x.Count),
                    Failure = failure.Sum(x => x.Count)
                });
            }

            var days = getStats.Select(x => x.Date).ToList();
            var successCount = getStats.Select(x => x.Success).ToList();
            var failureCount = getStats.Select(x => x.Failure).ToList();

            return new GetResult
            {
                Days = days,
                SuccessCount = successCount,
                FailureCount = failureCount
            };
        }
    }
}
