using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Gift
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class GiftViewController : ControllerBase
    {
        private const string Route = "giftView";


        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IPointShopRepository _pointShopRepository;
        private readonly IPointShopUserRepository _pointShopUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganManager _organManager;


        public GiftViewController(IConfigRepository configRepository, IUserRepository userRepository, IOrganManager organManager,
            IAuthManager authManager, IPointShopRepository pointShopRepository, IPointShopUserRepository pointShopUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _userRepository = userRepository;
            _pointShopRepository = pointShopRepository;
            _pointShopUserRepository = pointShopUserRepository;
        }

        public class GetResult
        {
            public PointShop Item { get; set; }
        }

        public class GetPayRequest
        {
            public int Id { get; set; }
            public PointShopType ShopType { get; set; }
            public string LinkMan { get; set; }
            public string LinkTel { get; set; }
            public string LinkAddress { get; set; }
        }

    }
}
