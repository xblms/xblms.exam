using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_PointShopUser")]
    public class PointShopUser : Entity
    {
        [DataColumn]
        public int ShopId { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public string Contacts { get; set; }
        [DataColumn]
        public string ContactMobile { get; set; }
        [DataColumn]
        public string ContactAddress { get; set; }
        [DataColumn]
        public PointShopType ShopType { get; set; }
        [DataColumn]
        public PointShopState UserState { get; set; }
        [DataColumn]
        public PointShopState AdminState { get; set; }
        [DataColumn]
        public string Remark { get; set; }

        /// <summary>
        /// 删除商品之后订单里面仍然可以预览
        /// </summary>
        public PointShop Shop { get; set; }
        [DataColumn]
        public int Point { get; set; }
        [DataColumn]
        public string ShopName { get; set; }
    }
}
