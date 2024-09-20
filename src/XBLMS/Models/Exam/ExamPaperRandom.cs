using System;
using System.Collections.Generic;
using Datory;
using Datory.Annotations;
using XBLMS.Enums;
namespace XBLMS.Models
{
    [DataTable("exam_PaperRandom")]
    public class ExamPaperRandom : Entity
    {
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
    }
}
