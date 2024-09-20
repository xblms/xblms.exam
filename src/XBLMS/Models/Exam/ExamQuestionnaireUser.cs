using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_QuestionnaireUser")]
    public class ExamQuestionnaireUser : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public SubmitType SubmitType { get; set; }

    }
}
