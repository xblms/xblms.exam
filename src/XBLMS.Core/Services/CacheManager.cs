using CacheManager.Core;

namespace XBLMS.Core.Services
{
    public partial class CacheManager : XBLMS.Services.ICacheManager
    {
        private readonly ICacheManager<object> _cacheManager;

        public CacheManager(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public IReadOnlyCacheManagerConfiguration Configuration => _cacheManager.Configuration;
    }
}
