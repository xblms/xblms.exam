using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory.Annotations;
using Newtonsoft.Json.Converters;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class OrganManager : IOrganManager
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganCompanyRepository _companyRepository;
        private readonly IOrganDepartmentRepository _departmentRepository;
        private readonly IOrganDutyRepository _dutyRepository;
        public OrganManager(IOrganCompanyRepository companyRepository, IOrganDepartmentRepository departmentRepository, IAdministratorRepository administratorRepository, IUserRepository userRepository , IOrganDutyRepository dutyRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
            _dutyRepository = dutyRepository;
        }

    }
}
