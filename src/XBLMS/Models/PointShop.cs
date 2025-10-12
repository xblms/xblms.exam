using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_PointShop")]
    public class PointShop : Entity
    {
        [DataColumn]
        public string Name { get; set; }
        [DataColumn(Text =true)]
        public string Subject { get; set; }
        [DataColumn]
        public int Point { get; set; }
        [DataColumn]
        public List<string> CoverImg { get; set; }
        [DataColumn]
        public int ShopTotal { get; set; }
        [DataColumn]
        public bool OnlyCompany { get; set; }
        [DataColumn]
        public PointShopType ShopType { get; set; } = PointShopType.OnSelf;
        [DataColumn]
        public string OffLineAddress { get; set; }
        [DataColumn]
        public bool Locked { get; set; } = false;
    }
}
