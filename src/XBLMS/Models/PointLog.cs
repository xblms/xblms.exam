using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_PointsLog")]
    public class PointLog : Entity
    {
        [DataColumn]
        public int UserId { get; set; }

        [DataColumn]
        public PointType PointType { get; set; }
        [DataColumn]
        public int ObjectId { get; set; }
        [DataColumn]
        public string ObjectName { get; set; }
        [DataColumn]
        public string Subject { get; set; }
        [DataColumn]
        public int Point { get; set; }
        /// <summary>
        /// 2025-05-05
        /// </summary>
        [DataColumn]
        public string DateStr { get; set; }
        /// <summary>
        /// 已通知
        /// </summary>
        [DataColumn]
        public bool IsNotice { get; set; }
    }
}
