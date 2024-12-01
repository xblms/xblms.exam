using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PkUser")]
    public class ExamPkUser : Entity
    {
        [DataColumn]
        public int PkId { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int AnswerTotal { get; set; }
        [DataColumn]
        public int RightTotal { get; set; }
        [DataColumn]
        public int DurationTotal { get; set; }
    }
}
