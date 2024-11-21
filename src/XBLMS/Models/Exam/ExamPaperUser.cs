using Datory;
using Datory.Annotations;
using System;

namespace XBLMS.Models
{
    [DataTable("exam_PaperUser")]
    public class ExamPaperUser : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamTimes { get; set; }
        [DataColumn]
        public DateTime? ExamBeginDateTime { get; set; }
        [DataColumn]
        public DateTime? ExamEndDateTime { get; set; }
        [DataColumn]
        public bool Moni { get; set; }
        [DataColumn]
        public bool Locked { get; set; } 

    }
}
