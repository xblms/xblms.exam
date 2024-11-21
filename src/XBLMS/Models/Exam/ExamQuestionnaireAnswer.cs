using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_QuestionnaireAnswer")]
    public class ExamQuestionnaireAnswer : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public string Answer { get; set; }
    }
}
