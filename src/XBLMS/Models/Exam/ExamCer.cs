using Datory;
using Datory.Annotations;
using System.Collections.Generic;

namespace XBLMS.Models
{
    [DataTable("exam_Cer")]
    public class ExamCer : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public string Prefix { get; set; }
        [DataColumn]
        public string OrganName { get; set; }
        [DataColumn]
        public string BackgroundImg { get; set; }
        [DataColumn]
        public List<string> MarkList { get; set; }
        [DataColumn]
        public int FontSize { get; set; }

    }
}
