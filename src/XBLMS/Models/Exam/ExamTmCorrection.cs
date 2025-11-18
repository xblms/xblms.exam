using Datory;
using Datory.Annotations;
using System;
using System.Text.Json.Serialization;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_TmCorrection")]
    public class ExamTmCorrection : Entity
    {
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public string TmTitle { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public string Reason { get; set; }
        [DataColumn]
        public ExamTmCorrectionAuditType AuditStatus { get; set; }
        [DataColumn]
        public string AuditResaon { get; set; }
        [DataColumn]
        public DateTime? AuditDate { get; set; }
        [DataColumn]
        public int AuditAdminId { get; set; }
        [JsonIgnore]
        [DataColumn(Text = true)]
        public string TmSourceObject { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
    }
}
