using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Dto
{
    public class ExamPaperOptions
    {
        public List<ExamPaperOptionsTx> Options { get; set; }
    }
    public class ExamPaperOptionsTx
    {
        public int TxId { get; set; }
        public ExamPaperOptionsTxNandu NanduTmCount { get; set; }
    }
    public class ExamPaperOptionsTxNandu
    {
        public int Nandu1TmCount { get; set; }
        public int Nandu2TmCount { get; set; }
        public int Nandu3TmCount { get; set; }
        public int Nandu4TmCount { get; set; }
        public int Nandu5TmCount { get; set; }
    }
}
