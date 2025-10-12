using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("study_CourseFilesGroup")]
    public class StudyCourseFilesGroup : Entity
    {
        [DataColumn]
        public string GroupName { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
    }
}
