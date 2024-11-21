using System.Collections.Generic;

namespace XBLMS.Dto
{
    public class OrganTree
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string OrganType { get; set; }
        public string OrganTypeName { get; set; }
        public int AdminCount { get; set; }
        public int AdminAllCount { get; set; }
        public int UserCount { get; set; }
        public int UserAllCount { get; set; }
        public List<OrganTree> Children { get; set; }
    }
}
