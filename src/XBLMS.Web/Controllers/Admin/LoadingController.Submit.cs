using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class LoadingController
    {
        [HttpPost, Route(Route)]
        public ActionResult<StringResult> Submit([FromBody] SubmitRequest request)
        {
            return new StringResult
            {
                Value = _settingsManager.Decrypt(request.RedirectUrl)
            };
        }
    }
}
