using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_Log")]
    public class Log: Entity
    {
        [DataColumn]
        public int AdminId { get; set; }

        [DataColumn]
        public int UserId { get; set; }

        [DataColumn]
        public string IpAddress { get; set; }

        [DataColumn]
        public string Action { get; set; }

        [DataColumn]
        public string Summary { get; set; }
    }
}
