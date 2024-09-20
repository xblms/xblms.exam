using CacheManager.Core;

namespace XBLMS.Services
{
    public partial interface ICacheManager
    {
        IReadOnlyCacheManagerConfiguration Configuration { get; }
    }
}