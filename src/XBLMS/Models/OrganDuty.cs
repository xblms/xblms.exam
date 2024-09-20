using System.Collections.Generic;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_Duty")]
    public class OrganDuty : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        public IList<OrganDuty> Children { get; set; }

    }
}
