using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("exam_PracticeWrong")]
    public class ExamPracticeWrong : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }
    }
}
