using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteLoginLog)]
        public async Task<ActionResult<GetLoginLogResult>> GetLoginLog([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.LogRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            return new GetLoginLogResult
            {
                Total = total,
                List = list
            };
        }

        public class GetLoginLogResult
        {
            public int Total { get; set; }
            public List<Log> List { get; set; }
        }
    }
}
