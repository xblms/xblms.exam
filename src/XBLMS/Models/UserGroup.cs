using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_UserGroup")]
    public class UserGroup : Entity
    {
        [DataColumn]
        public string GroupName { get; set; }
        [DataColumn]
        public UsersGroupType GroupType { get; set; }
        [DataColumn(Text = true)]
        public List<int> UserIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> CompanyIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> DepartmentIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> DutyIds { get; set; }
        [DataColumn]
        public int UserTotal { get; set; }
        [DataColumn(Text = true)]
        public string Description { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
    }
}
