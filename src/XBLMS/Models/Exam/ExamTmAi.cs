using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_TmAi")]
    public class ExamTmAi : ExamTm
    {
        [DataColumn]
        public bool Stocked { get; set; }
    }
}
