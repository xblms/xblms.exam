using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class EditorTmWordOpenLayerController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteCheck)]
        public async Task<GetCheckResult> Check([FromBody] GetRequest reqeust)
        {
            var admin = await _authManager.GetAdminAsync();

            var (total, successTotal, errorTotal, tmList, resultHtml) = await Check(reqeust.TmHtml, reqeust.TreeId, admin);

            return new GetCheckResult()
            {
                Total=total,
                SuccessTotal=successTotal,
                ErrorTotal=errorTotal,
                ResultHtml=resultHtml
            };
        }
    }
}
