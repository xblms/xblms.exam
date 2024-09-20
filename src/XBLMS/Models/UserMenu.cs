using System.Collections.Generic;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_UserMenu")]
    public class UserMenu : Entity
    {

        [DataColumn]
        public bool Disabled { get; set; }

        [DataColumn]
        public int ParentId { get; set; }

        [DataColumn]
        public int Taxis { get; set; }

        [DataColumn]
        public string Text { get; set; }

        [DataColumn]
        public string IconClass { get; set; }

        [DataColumn]
        public string Link { get; set; }

        [DataColumn]
        public string Target { get; set; }
    }
}
