using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class BlockController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<AuthResult>> Auth([FromBody] AuthRequest request)
        {
            var sessionId = string.Empty;
            if (await _configRepository.IsNeedInstallAsync())
            {
                return new AuthResult
                {
                    Success = true,
                    SessionId = sessionId
                };
            }
  

            var rules = await _ruleRepository.GetAllAsync(2);
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    if (rule.Password == request.Password)
                    {
                        sessionId = _settingsManager.Encrypt(rule.Password);
                        break;
                    }
                }
            }
            
            return new AuthResult
            {
                Success = !string.IsNullOrEmpty(sessionId),
                SessionId = sessionId
            };
        }
    }
}
