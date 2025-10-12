using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("exam_Tm")]
    public class ExamTm : Entity
    {
        [DataColumn]
        public int TreeId { get; set; }
        [DataColumn]
        public int TxId { get; set; }
        [DataColumn(Text = true)]
        public string Title { get; set; }
        [DataColumn(Text = true)]
        public string Answer { get; set; }
        [DataColumn]
        public decimal Score { get; set; }
        [DataColumn]
        public string Zhishidian { get; set; }
        [DataColumn]
        public int Nandu { get; set; }
        [DataColumn(Text = true)]
        public string Jiexi { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
        [DataColumn]
        public int UseCount { get; set; }
        [DataColumn]
        public int AnswerCount { get; set; }
        [DataColumn]
        public int RightCount { get; set; }
        [DataColumn]
        public int WrongCount { get; set; }
        [DataColumn]
        public List<string> TreeParentPath { get; set; }
        [DataColumn]
        public List<string> TmGroupIds { get; set; }
    }
}
