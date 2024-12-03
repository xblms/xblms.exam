using Datory;
using Datory.Annotations;
using System;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_AssessmentUser")]
    public class ExamAssessmentUser : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamAssId { get; set; }
        [DataColumn]
        public SubmitType SubmitType { get; set; }
        [DataColumn]
        public DateTime? ExamBeginDateTime { get; set; }
        [DataColumn]
        public DateTime? ExamEndDateTime { get; set; }
        [DataColumn]
        public int ConfigId { get; set; }
        [DataColumn]
        public string ConfigName { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
        [DataColumn]
        public int TotalScore { get; set; }

    }
}
