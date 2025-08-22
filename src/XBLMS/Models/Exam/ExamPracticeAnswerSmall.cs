using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PracticeAnswerSmall")]
    public class ExamPracticeAnswerSmall : ExamPracticeAnswer
    {
        [DataColumn]
        public int AnswerId { get; set; }

    }
}
