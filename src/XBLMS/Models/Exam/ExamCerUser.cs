using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_CerUser")]
    public class ExamCerUser : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int CerId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamStartId { get; set; }
        [DataColumn]
        public string CerNumber { get; set; }
        [DataColumn]
        public DateTime? CerDateTime { get; set; }
        [DataColumn]
        public string CerImg { get; set; }
    }
}
