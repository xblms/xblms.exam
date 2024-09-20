using System.Collections.Generic;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_Department")]
    public class OrganDepartment : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        public IList<OrganDepartment> Children { get; set; }

    }
}
