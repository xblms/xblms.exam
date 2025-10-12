using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {
        [HttpPost, Route(RouteCache)]
        public async Task<ActionResult<IntResult>> Cache()
        {
            int cacheTotal = 0;
            var adminAuth = await _authManager.GetAdminAuth();

            var admin = await _authManager.GetAdminAsync();
            if (admin != null && admin.Auth == AuthorityType.Admin)
            {
                var config = await _configRepository.GetAsync();
                if (config.ExamTmCache)
                {
                    _examTmRepository.CacheRemoveListAsync(adminAuth.CurCompanyId);
                    var list = _examTmRepository.CacheGetListAsync(adminAuth.CurCompanyId);
                    if (list != null && list.Count > 0)
                    {
                        cacheTotal = list.Count;
                    }
                    if (cacheTotal > 0)
                    {
                        return this.Error("操作失败，请重启缓存服务器");
                    }
                }
                else
                {
                    var list = await _examTmRepository.CacheSetListAsync(adminAuth.CurCompanyId);
                    if (list != null && list.Count > 0)
                    {
                        cacheTotal = list.Count;
                    }
                    if (cacheTotal == 0)
                    {
                        return this.Error("操作失败，请重启缓存服务器");
                    }
                }
                config.ExamTmCache = !config.ExamTmCache;
                await _configRepository.UpdateAsync(config);

            }
            else
            {
                return this.NoAuth();
            }
            return new IntResult
            {
                Value = cacheTotal
            };
        }
    }
}
