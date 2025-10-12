using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseEvaluation")]
    public class StudyCourseEvaluation : Entity
    {
        [DataColumn]
        public string Title { get; set; }
        [DataColumn]
        public int MaxStar { get; set; }
        [DataColumn]
        public int TotalScore { get; set; }
        [DataColumn]
        public int TotalItem { get; set; }
        [DataColumn]
        public bool Locked { get; set; }

    }
}
