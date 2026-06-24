using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UserLayerViewController : ControllerBase
    {
        private const string Route = "common/userLayerView";

        private readonly IUserRepository _userRepository;
        private readonly IOrganManager _organManager;
        private readonly IConfigRepository _configRepository;

        public UserLayerViewController(IUserRepository userRepository, IOrganManager organManager, IConfigRepository configRepository)
        {
            _userRepository = userRepository;
            _organManager = organManager;
            _configRepository = configRepository;
        }

        public class GetRequest
        {
            public string Guid { get; set; }
        }

        public class GetResult
        {
            public User User { get; set; }
            public string GroupName { get; set; }
            public SystemCode SystemCode { get; set; }
        }
    }
}
