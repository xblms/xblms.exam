using System;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_BlockAnalysis")]
    public class BlockAnalysis : Entity
    {

        [DataColumn]
        public DateTime BlockDate { get; set; }

        [DataColumn]
        public int BlockCount { get; set; }
        [DataColumn]
        public int BlockType { get; set; } = 1;
    }
}
