using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class SelectCompanyController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var resultCompanyss = new List<OrganCompany>();
            var companys = await _organManager.GetCompanyListAsync();
            if (companys != null && companys.Count > 0)
            {
                foreach (var company in companys)
                {
                    var pathName = await _organManager.GetOrganName(0, 0, company.Id);
                    var allIds = await _organManager.GetCompanyIdsAsync(company.Id);
                    company.Set("PathNames", pathName);
                    company.Set("AdminCount", $"{await _administratorRepository.GetCountAsync(company.Id, 0, 0)}/{await _administratorRepository.GetCountAsync(allIds, null, null)}");
                    company.Set("UserCount", $"{await _userRepository.GetCountAsync(company.Id, 0, 0)}/{await _userRepository.GetCountAsync(allIds, null, null)}");
                    if (!string.IsNullOrEmpty(request.Search))
                    {
                        if (company.Name.Contains(request.Search) || pathName.Contains(request.Search))
                        {
                            resultCompanyss.Add(company);
                        }
                    }
                    else
                    {
                        resultCompanyss.Add(company);
                    }


                }
            }
            return new GetResult
            {
                Companys = resultCompanyss
            };
        }
    }
}
