using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_Knowledges")]
    public class Knowledges : Entity
    {
        [DataColumn]
        public int TreeId { get; set; }
        [DataColumn]
        public List<string> TreeParentPath { get; set; }
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public string Url { get; set; }
        [DataColumn]
        public string CoverImgUrl { get; set; }
        [DataColumn]
        public List<string> Zsds { get; set; }
        [DataColumn]
        public int Likes { get; set; }
        [DataColumn]
        public int Collects { get; set; }
        [DataColumn]
        public List<string> LikeUserIds { get; set; }
        [DataColumn]
        public List<string> CollectUserIds { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
    }
}
