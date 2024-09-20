using System.Collections.Generic;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_Company")]
    public class OrganCompany : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        public IList<OrganCompany> Children { get; set; }

    }
}
