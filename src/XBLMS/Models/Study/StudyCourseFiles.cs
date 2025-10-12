using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseFiles")]
    public class StudyCourseFiles : Entity
    {
        [DataColumn]
        public int GroupId { get; set; }

        [DataColumn]
        public string FileName { get; set; }

        [DataColumn]
        public string FileType { get; set; }

        [DataColumn]
        public string Url { get; set; }
        [DataColumn]
        public string CoverUrl { get; set; }
        [DataColumn]
        public int FileSize { get; set; }
        [DataColumn]
        public int Duration { get; set; }
    }
}
