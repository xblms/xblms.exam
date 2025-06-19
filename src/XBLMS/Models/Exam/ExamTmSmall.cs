using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_TmSmall")]
    public class ExamTmSmall : ExamTm
    {
        [DataColumn]
        public int ParentId { get; set; }
    }
}
