using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesEncryptController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return Unauthorized();
            }

            var encoded = request.IsEncrypt
                ? _settingsManager.Encrypt(request.Value)
                : _settingsManager.Decrypt(request.Value);

            if (!request.IsEncrypt && string.IsNullOrEmpty(encoded))
            {
                return this.Error("指定的字符串为非系统加密的字符串");
            }

            return new StringResult
            {
                Value = encoded
            };
        }
    }
}
