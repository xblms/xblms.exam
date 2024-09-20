using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PracticeAnswer")]
    public class ExamPracticeAnswer : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int PracticeId { get; set; }
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public string Answer { get; set; }
        [DataColumn]
        public bool IsRight { get; set; }

    }
}
