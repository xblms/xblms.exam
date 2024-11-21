using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_PaperAnswer")]
    public class ExamPaperAnswer : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamStartId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int RandomTmId { get; set; }
        [DataColumn]
        public string Answer { get; set; }
        [DataColumn]
        public decimal Score { get; set; }
        [DataColumn]
        public ExamTmType ExamTmType { get; set; }
    }
}
