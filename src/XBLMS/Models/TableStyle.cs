using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_TableStyle")]
    public class TableStyle : Entity
    {
        [DataColumn]
        public TableStyleType Type { get; set; }
        [DataColumn]
        public int RelatedIdentity { get; set; }

        [DataColumn]
        public string TableName { get; set; }

        [DataColumn] 
        public string AttributeName { get; set; }

        [DataColumn] 
        public int Taxis { get; set; }

        [DataColumn] 
        public string DisplayName { get; set; }

        [DataColumn] 
        public string HelpText { get; set; }

        [DataColumn]
        public bool List { get; set; }

        [DataColumn] 
        public InputType InputType { get; set; }

        [DataColumn] 
        public string DefaultValue { get; set; }

        [DataColumn]
        public bool Horizontal { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
        [DataColumn]
        public bool Valid { get; set; } = true;
        [DataIgnore]
        public List<InputStyleItem> Items { get; set; }

        [DataIgnore]
        public List<InputStyleRule> Rules { get; set; }

        [DataIgnore]
        public bool IsSystem { get; set; }

        public string ItemValues { get; set; }

        public string RuleValues { get; set; }

        public int Height { get; set; }

        public bool IsFormatString { get; set; }

        public string CustomizeCode { get; set; }
    }
}
