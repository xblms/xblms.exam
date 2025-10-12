using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_Company")]
    public class OrganCompany : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public List<string> ParentNames { get; set; }
    }
}
