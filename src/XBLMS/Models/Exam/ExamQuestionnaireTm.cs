using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_QuestionnaireTm")]
    public class ExamQuestionnaireTm : Entity
    {
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn(Text = true)]
        public string Title { get; set; }
        [DataColumn]
        public ExamQuestionnaireTxType Tx { get; set; }
    }
}
