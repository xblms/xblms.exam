using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_Pk")]
    public class ExamPk : Entity
    {
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public PkRuleType RuleType { get; set; }
        [DataColumn]
        public int Vs { get; set; }
        /// <summary>
        /// 第几轮
        /// </summary>
        [DataColumn]
        public int Current { get; set; }
        [DataColumn]
        public DateTime? PkBeginDateTime { get; set; } = DateTime.Now;
        [DataColumn]
        public DateTime? PkEndDateTime { get; set; } = DateTime.Now.AddDays(3);
        [DataColumn]
        public int UserGroupId { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmGroupIds { get; set; }
        [DataColumn]
        public bool Finished { get; set; }
        [DataColumn]
        public string Mark { get; set; }
    }
}
