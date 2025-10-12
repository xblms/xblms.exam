using System.Collections.Generic;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Dto
{
    public class AdminAuth
    {
        public Administrator Admin { get; set; }
        public AuthorityType AuthType { get; set; }
        public AuthorityDataType AuthDataType { get; set; }
        public bool AuthDataShowAll { get; set; }
        public int AdminId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int CurCompanyId { get; set; }
        public List<string> CompanyParentPath { get; set; }
        public List<string> DepartmentParentPath { get; set; }

    }
}
