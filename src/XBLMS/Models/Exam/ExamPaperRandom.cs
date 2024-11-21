using Datory;
using Datory.Annotations;
namespace XBLMS.Models
{
    [DataTable("exam_PaperRandom")]
    public class ExamPaperRandom : Entity
    {
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
    }
}
