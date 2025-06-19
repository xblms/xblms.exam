using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PaperAnswerSmall")]
    public class ExamPaperAnswerSmall : ExamPaperAnswer
    {
        [DataColumn]
        public int AnswerId { get; set; }
    }
}
