using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_Practice")]
    public class ExamPractice : Entity
    {
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public string Title { get; set; }
        [DataColumn]
        public bool IsCreate { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public PracticeType PracticeType { get; set; }
        [DataColumn(Text = true)]
        public List<string> Zsds { get; set; }
        [DataColumn]
        public List<int> Nds { get; set; }
        [DataColumn]
        public List<int> TxIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }
        [DataColumn]
        public int TmCount { get; set; }
        [DataColumn]
        public int MineTmCount { get; set; }
        [DataColumn]
        public int AnswerCount { get; set; }
        [DataColumn]
        public int RightCount { get; set; }
    }
}
