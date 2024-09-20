using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class SelectDepartmentController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var resultDepartments = new List<OrganDepartment>();
            var departments = await _organManager.GetDepartmentListAsync();
            if (departments != null && departments.Count > 0)
            {
                foreach (var department in departments)
                {
                    var pathName = await _organManager.GetOrganName(0, department.Id, department.CompanyId);
                    var allIds = await _organManager.GetDepartmentIdsAsync(department.Id);
                    department.Set("PathNames", pathName);
                    department.Set("AdminCount", $"{await _administratorRepository.GetCountAsync(department.CompanyId, department.Id, 0)}/{await _administratorRepository.GetCountAsync(null, allIds, null)}");
                    department.Set("UserCount", $"{await _userRepository.GetCountAsync(department.CompanyId, department.Id, 0)}/{await _userRepository.GetCountAsync(null, allIds, null)}");
                    if (!string.IsNullOrEmpty(request.Search))
                    {
                        if (department.Name.Contains(request.Search) || pathName.Contains(request.Search))
                        {
                            resultDepartments.Add(department);
                        }
                    }
                    else
                    {
                        resultDepartments.Add(department);
                    }


                }
            }
            return new GetResult
            {
                Departments = resultDepartments
            };
        }
    }
}
