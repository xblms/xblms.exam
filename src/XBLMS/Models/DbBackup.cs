using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_DbBackup")]
    public class DbBackup : Entity
    {
        [DataColumn]
        public DateTime? BeginTime { get; set; }
        [DataColumn]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 0 执行中 1 ok 2error
        /// </summary>
        [DataColumn]
        public int Status { get; set; }
        [DataColumn]
        public string FileName { get; set; }
        [DataColumn]
        public string FilePath { get; set; }
        [DataColumn]
        public string DataSize { get; set; }
        [DataColumn(Text = true)]
        public List<string> SuccessTables { get; set; }
        [DataColumn(Text = true)]
        public List<string> ErrorTables { get; set; }
        [DataColumn(Text = true)]
        public string ErrorLog { get; set; }

    }
}
