using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("xblms_CrudDemo")]
    public class CrudDemo : Entity
    {
        [DataColumn]
        public string title { get; set; }

    }
}
