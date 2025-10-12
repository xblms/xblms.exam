using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseEvaluationItemUser")]
    public class StudyCourseEvaluationItemUser : Entity
    {
        [DataColumn]
        public int PlanId { get; set; }
        [DataColumn]
        public int CourseId { get; set; }
        [DataColumn]
        public int EvaluationId { get; set; }
        [DataColumn]
        public int EvaluationItemId { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int StarValue { get; set; }
        [DataColumn]
        public string TextContent { get; set; }
    }
}
