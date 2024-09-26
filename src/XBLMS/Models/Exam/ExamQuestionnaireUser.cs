using Datory;
using Datory.Annotations;
using System;
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
        [DataColumn]
        public DateTime? ExamBeginDateTime { get; set; }
        [DataColumn]
        public DateTime? ExamEndDateTime { get; set; }
        [DataColumn]
        public bool Locked { get; set; }

    }
}
