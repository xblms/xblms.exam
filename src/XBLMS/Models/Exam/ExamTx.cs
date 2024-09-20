using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_Tx")]
    public class ExamTx : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public ExamTxBase ExamTxBase { get; set; }
        [DataColumn]
        public decimal Score { get; set; }
        [DataColumn]
        public int Taxis { get; set; }

    }
}
