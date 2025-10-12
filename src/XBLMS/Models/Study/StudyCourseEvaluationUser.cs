using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseEvaluationUser")]
    public class StudyCourseEvaluationUser : Entity
    {
        [DataColumn]
        public int PlanId { get; set; }
        [DataColumn]
        public int CourseId { get; set; }
        [DataColumn]
        public int EvaluationId { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int TotalStarValue { get; set; }
    }
}
