using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PaperRandomTmSmall")]
    public class ExamPaperRandomTmSmall : ExamTm
    {
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public int SourceTmId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamPaperRandomId { get; set; }

    }
}
