using Datory;
using Datory.Annotations;
namespace XBLMS.Models
{
    [DataTable("exam_AssessmentConfigSet")]
    public class ExamAssessmentConfigSet : Entity
    {
        [DataColumn]
        public int ConfigId { get; set; }
        [DataColumn]
        public string Result { get; set; }
        [DataColumn]
        public int MinScore { get; set; }
        [DataColumn]
        public int MaxScore { get; set; }
    }
}
