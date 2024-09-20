using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("xblms_Role")]
    public class Role : Entity
    {
        [DataColumn]
        public string RoleName  { get; set; }

        [DataColumn(Text =true)]
        public List<string> MenuIds { get; set; }
        [DataColumn(Text = true)]
        public List<string> PermissionIds { get; set; }
        [DataColumn(Text = true)]
        public List<string> SelectIds { get; set; }

        [DataColumn]
        public string Description { get; set; }
    }
}
