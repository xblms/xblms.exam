using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_DbRecover")]
    public class DbRecover : Entity
    {
        [DataColumn]
        public int JobId { get; set; }
        [DataColumn]
        public DateTime? BeginTime { get; set; }
        [DataColumn]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 0 执行中 1 ok 2error
        /// </summary>
        [DataColumn]
        public int Status { get; set; }
        [DataColumn(Text = true)]
        public List<string> SuccessTables { get; set; }
        [DataColumn(Text = true)]
        public List<string> ErrorTables { get; set; }
        [DataColumn(Text = true)]
        public string ErrorLog { get; set; }

    }
}
