using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_PlanCourse")]
    public class StudyPlanCourse : Entity
    {
        [DataColumn]
        public int PlanId { get; set; }
        [DataColumn]
        public int CourseId { get; set; }
        [DataColumn]
        public string CourseName { get; set; }
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
        public bool IsSelectCourse { get; set; }
        [DataColumn]
        public bool OffLine { get; set; }
        /// <summary>
        /// 学时
        /// </summary>
        [DataColumn]
        public decimal StudyHour { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        [DataColumn]
        public decimal Credit { get; set; }
        [DataColumn]
        public int Duration { get; set; }
        [DataColumn]
        public int Taxis { get; set; }
    }
}
