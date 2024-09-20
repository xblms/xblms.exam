using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_DbCache")]
    public class DbCache : Entity
    {
        [DataColumn]
        public string CacheKey { get; set; }

        [DataColumn]
        public string CacheValue { get; set; }
    }
}
