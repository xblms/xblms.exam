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
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var (total, successTotal, errorTotal, tmList, resultHtml) = await Check(reqeust.TmHtml, reqeust.TreeId, admin, adminAuth);

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
