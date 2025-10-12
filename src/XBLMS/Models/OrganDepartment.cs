using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_Department")]
    public class OrganDepartment : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        [DataColumn]
        public string SyncId { get; set; }
        [DataColumn]
        public List<string> ParentNames { get; set; }
    }
}
