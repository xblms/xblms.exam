using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("exam_PaperRandomConfig")]
    public class ExamPaperRandomConfig : Entity
    {
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int TxId { get; set; }
        [DataColumn]
        public string TxName { get; set; }
        [DataColumn]
        public decimal TxScore { get; set; }
        [DataColumn]
        public int Nandu1TmCount { get; set; } = 0;
        [DataColumn]
        public int Nandu2TmCount { get; set; } = 0;
        [DataColumn]
        public int Nandu3TmCount { get; set; } = 0;
        [DataColumn]
        public int Nandu4TmCount { get; set; } = 0;
        [DataColumn]
        public int Nandu5TmCount { get; set; } = 0;
        [DataColumn]
        public int Nandu1TmTotal { get; set; }
        [DataColumn]
        public int Nandu2TmTotal { get; set; }
        [DataColumn]
        public int Nandu3TmTotal { get; set; }
        [DataColumn]
        public int Nandu4TmTotal { get; set; }
        [DataColumn]
        public int Nandu5TmTotal { get; set; }
        [DataColumn]
        public int TxTaxis { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }
        [DataColumn]
        public decimal ScoreTotal { get; set; }
        [DataColumn]
        public int TmTotal { get; set; }
    }
}
