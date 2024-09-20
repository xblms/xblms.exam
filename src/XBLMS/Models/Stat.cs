using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_Stat")]
    public class Stat : Entity
    {
        [DataColumn]
        public StatType StatType { get; set; }

        [DataColumn]
        public int AdminId { get; set; }

        [DataColumn]
        public int Count { get; set; }
    }
}
