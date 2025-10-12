using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseWare")]
    public class StudyCourseWare : Entity
    {
        [DataColumn]
        public int CourseId { get; set; }
        [DataColumn]
        public int CourseFileId { get; set; }
        [DataColumn]
        public int Taxis { get; set; }
        [DataColumn]
        public string FileName { get; set; }
        [DataColumn]
        public string Url { get; set; }
        [DataColumn]
        public int Duration { get; set; }
    }
}
