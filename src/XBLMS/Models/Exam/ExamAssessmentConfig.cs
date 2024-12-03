using Datory;
using Datory.Annotations;
namespace XBLMS.Models
{
    [DataTable("exam_AssessmentConfig")]
    public class ExamAssessmentConfig : Entity
    {
        [DataColumn]
        public string Title { get; set; }
        [DataColumn]
        public bool Locked { get; set; } = false;
    }
}
