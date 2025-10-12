using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseEvaluationItem")]
    public class StudyCourseEvaluationItem : Entity
    {
        [DataColumn]
        public int EvaluationId { get; set; }
        [DataColumn]
        public string Title { get; set; }
        [DataColumn]
        public bool TextContent { get; set; }
        [DataColumn]
        public int Taxis { get; set; }
    }
}
