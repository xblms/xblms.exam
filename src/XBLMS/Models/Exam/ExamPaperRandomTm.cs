using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PaperRandomTm")]
    public class ExamPaperRandomTm : ExamTm
    {
        [DataColumn]
        public int SourceTmId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamPaperRandomId { get; set; }

    }
}
