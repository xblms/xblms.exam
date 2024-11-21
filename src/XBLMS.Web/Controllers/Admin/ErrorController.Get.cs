using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class ErrorController
    {
        [HttpGet, Route(Route)]
        public async Task<GetResult> Get([FromQuery] int logId)
        {
            var error = await _errorLogRepository.GetErrorLogAsync(logId);

            return new GetResult
            {
                Message = StringUtils.Trim(error.Message),
                StackTrace = StringUtils.Trim(error.StackTrace),
                Summary = StringUtils.Trim(error.Summary),
                CreatedDate = error.CreatedDate,
            };
        }
    }
}
