using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_PaperStart")]
    public class ExamPaperStart : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamPaperRandomId { get; set; }
        [DataColumn]
        public DateTime? BeginDateTime { get; set; }
        [DataColumn]
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 已用时 秒
        /// </summary>
        [DataColumn]
        public long ExamTimeSeconds { get; set; }
        [DataColumn]
        public int MarkTeacherId { get; set; }
        [DataColumn]
        public bool IsMark { get; set; }
        [DataColumn]
        public decimal Score { get; set; }
        [DataColumn]
        public bool IsSubmit { get; set; }
    }
}
