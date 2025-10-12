using Datory;
using Datory.Annotations;
using System;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("study_PlanUser")]
    public class StudyPlanUser : Entity
    {
        [DataColumn]
        public int PlanId { get; set; }
        [DataColumn]
        public int PlanYear { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public decimal Credit { get; set; }
        [DataColumn]
        public decimal TotalCredit { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
        [DataColumn]
        public DateTime? PlanBeginDateTime { get; set; }
        [DataColumn]
        public DateTime? PlanEndDateTime { get; set; }
        [DataColumn]
        public DateTime? BeginStudyDateTime { get; set; }
        [DataColumn]
        public DateTime? OverStudyDateTime { get; set; }
        [DataColumn]
        public DateTime? LastStudyDateTime { get; set; }
        [DataColumn]
        public StudyStatType State { get; set; }
    }
}
