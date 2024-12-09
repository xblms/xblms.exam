using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class LogoutController
    {
        [HttpPost, Route(Route)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BoolResult>> Submit()
        {
            var cacheKey = Constants.GetSessionIdCacheKey(_authManager.AdminId);
            await _dbCacheRepository.RemoveAsync(cacheKey);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
