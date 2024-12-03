using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_AssessmentTm")]
    public class ExamAssessmentTm : Entity
    {
        [DataColumn]
        public int ExamAssId { get; set; }
        [DataColumn(Text = true)]
        public string Title { get; set; }
        [DataColumn]
        public ExamAssessmentTxType Tx { get; set; }
    }
}
