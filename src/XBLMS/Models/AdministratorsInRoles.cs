using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_AdministratorsInRoles")]
    public class AdministratorsInRoles : Entity
    {
        [DataColumn]
        public int RoleId { get; set; }

        [DataColumn]
        public int AdminId { get; set; }
	}
}
