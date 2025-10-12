using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RoutePointLog)]
        public async Task<ActionResult<GetPointLogResult>> GetPointLog([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.PointLogRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            return new GetPointLogResult
            {
                Total = total,
                List = list
            };
        }

        public class GetPointLogResult
        {
            public int Total { get; set; }
            public List<PointLog> List { get; set; }
        }
    }
}
