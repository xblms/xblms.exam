using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_KnowledgesTree")]
    public class KnowledgesTree : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public List<string> ParentPath { get; set; }
    }
}
