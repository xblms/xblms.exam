using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_ScheduledTask")]
    public class ScheduledTask : Entity
    {
        [DataColumn]
        public string Title { get; set; }

        [DataColumn]
        public TaskType TaskType { get; set; }

        [DataColumn]
        public TaskInterval TaskInterval { get; set; }

        [DataColumn]
        public int Every { get; set; }

        [DataColumn]
        public List<int> Weeks { get; set; }

        [DataColumn]
        public DateTime StartDate { get; set; }

        [DataColumn]
        public bool IsDisabled { get; set; }

        [DataColumn]
        public int Timeout { get; set; }

        [DataColumn]
        public bool IsRunning { get; set; }

        [DataColumn]
        public DateTime? LatestStartDate { get; set; }

        [DataColumn]
        public DateTime? LatestEndDate { get; set; }

        [DataColumn]
        public bool IsLatestSuccess { get; set; }

        [DataColumn]
        public int LatestFailureCount { get; set; }

        [DataColumn]
        public string LatestErrorMessage { get; set; }

        [DataColumn]
        public DateTime? ScheduledDate { get; set; }
        [DataColumn]
        public string PingHost { get; set; }
        [DataColumn]
        public int PingCounts { get; set; }
    }
}
