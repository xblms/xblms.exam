using System;
using System.Collections.Generic;
using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PaperTree")]
    public class ExamPaperTree : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn]
        public int ParentId { get; set; }
        public List<ExamPaperTree> Children { get; set; }

    }
}
