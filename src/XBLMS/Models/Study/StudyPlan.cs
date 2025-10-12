using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("study_Plan")]
    public class StudyPlan : Entity
    {
        [DataColumn]
        public string PlanName { get; set; }
        [DataColumn(Text = true)]
        public string Description { get; set; }
        [DataColumn]
        public int PlanYear { get; set; } = DateTime.Now.Year;
        [DataColumn]
        public decimal PlanCredit { get; set; } = 100;
        [DataColumn]
        public DateTime? PlanBeginDateTime { get; set; } = DateTime.Now;
        [DataColumn]
        public DateTime? PlanEndDateTime { get; set; } = DateTime.Now.AddYears(1);
        [DataColumn]
        public string CoverImg { get; set; }
        [DataColumn]
        public int TotalDuration { get; set; }
        [DataColumn]
        public int TotalCount { get; set; }
        [DataColumn]
        public bool SelectCourseOverByCount { get; set; } = true;
        [DataColumn]
        public int SelectCourseOverCount { get; set; }
        [DataColumn]
        public bool SelectCourseOverByDuration { get; set; } = false;
        [DataColumn]
        public int SelectCourseOverMinute { get; set; }
        [DataColumn]
        public int SelectTotalDuration { get; set; }
        [DataColumn]
        public int SelectTotalCount { get; set; }
        [DataColumn]
        public List<int> UserGroupIds { get; set; }
        [DataColumn]
        public SubmitType SubmitType { get; set; } = SubmitType.Save;
        [DataColumn]
        public int ExamId { get; set; }
        [DataColumn]
        public string ExamName { get; set; }
        [DataColumn]
        public bool Locked { get; set; }

    }
}
