using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_TmAnalysisTm")]
    public class ExamTmAnalysisTm : ExamTm
    {
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public int AnalysisId { get; set; }
        [DataColumn]
        public decimal WrongPercent { get; set; }
        /// <summary>
        /// 置信度
        /// </summary>
        [DataColumn]
        public decimal WrongRate { get; set; }
    }
}
