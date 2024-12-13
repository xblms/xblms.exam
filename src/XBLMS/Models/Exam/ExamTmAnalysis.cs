using Datory;
using Datory.Annotations;
using System;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_TmAnalysis")]
    public class ExamTmAnalysis : Entity
    {
        [DataColumn]
        public DateTime? TmAnalysisDateTime { get; set; }
        [DataColumn]
        public TmAnalysisType TmAnalysisType { get; set; }
        [DataColumn]
        public int TmAnalysisExamPapaerId { get; set; }
    }
}
