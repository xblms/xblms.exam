using Datory;
using Datory.Annotations;
using Newtonsoft.Json;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_StatLog")]
    public class StatLog : Entity
    {
        [DataColumn]
        public StatType StatType { get; set; }
        [DataColumn]
        public string StatTypeStr { get; set; }

        [DataColumn]
        public int AdminId { get; set; }
        [DataColumn]
        public int ObjectId { get; set; }
        [DataColumn]
        public string ObjectName { get; set; }
        [DataColumn]
        public string IpAddress { get; set; }

        [JsonIgnore]
        [DataColumn(Text = true)]
        public string LastEntity { get; set; }
    }
}
