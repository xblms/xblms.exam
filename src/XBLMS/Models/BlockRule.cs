using System.Collections.Generic;
using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_BlockRule")]
    public class BlockRule : Entity
    {
        [DataColumn]
        public string RuleName { get; set; }

        [DataColumn]
        public AreaType AreaType { get; set; }

        [DataColumn]
        public List<int> BlockAreas { get; set; }

        [DataColumn]
        public List<string> AllowList { get; set; }

        [DataColumn]
        public List<string> BlockList { get; set; }

        [DataColumn]
        public bool IsAllChannels { get; set; }

        [DataColumn]
        public List<int> BlockChannels { get; set; }

        [DataColumn]
        public BlockMethod BlockMethod { get; set; }

        [DataColumn]
        public string RedirectUrl { get; set; }

        [DataColumn]
        public string Warning { get; set; }

        [DataColumn]
        public string Password { get; set; }
        /// <summary>
        /// 1 user 2 admin
        /// </summary>
        [DataColumn]
        public int BlockType { get; set; } = 1;
    }
}
