using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("study_Course")]
    public class StudyCourse : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public List<string> Mark { get; set; }
        [DataColumn(Text = true)]
        public string Description { get; set; }
        [DataColumn]
        public int TreeId { get; set; }
        [DataColumn]
        public List<string> TreeParentPath { get; set; }
        /// <summary>
        /// 课时
        /// </summary>
        [DataColumn]
        public decimal StudyHour { get; set; }
        [DataColumn]
        public int Duration { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        [DataColumn]
        public decimal Credit { get; set; }
        [DataColumn]
        public string CoverImg { get; set; }
        /// <summary>
        /// 公共课
        /// </summary>
        [DataColumn]
        public bool Public { get; set; }
        [DataColumn]
        public bool OnlyCompany { get; set; }
        /// <summary>
        /// 线下课
        /// </summary>
        [DataColumn]
        public bool OffLine { get; set; }
        [DataColumn]
        public DateTime? OfflineBeginDateTime { get; set; }
        [DataColumn]
        public DateTime? OfflineEndDateTime { get; set; }
        [DataColumn]
        public string OfflineAddress { get; set; }
        [DataColumn]
        public string OfflineTeacher { get; set; }
        [DataColumn]
        public int TeacherId { get; set; }
        [DataColumn]
        public string TeacherName { get; set; }

        [DataColumn]
        public int ExamId { get; set; }
        [DataColumn]
        public string ExamName { get; set; }
        [DataColumn]
        public int ExamQuestionnaireId { get; set; }
        [DataColumn]
        public string ExamQuestionnaireName { get; set; }
        [DataColumn]
        public int StudyCourseEvaluationId { get; set; }
        [DataColumn]
        public string StudyCourseEvaluationName { get; set; }
        [DataColumn]
        public int TotalUser { get; set; }
        [DataColumn]
        public int TotaEvaluationlUser { get; set; }
        [DataColumn]
        public int TotalEvaluation { get; set; }
        public decimal TotalAvgEvaluation { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
    }
}
