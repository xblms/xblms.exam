using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory;
using Datory.Annotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Converters;
using NPOI.POIFS.Properties;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<OrganDuty> GetDutyAsync(int companyId,int departmentId,string name)
        {
            return await _dutyRepository.GetAsync(companyId, departmentId, name);
        }
        public async Task<OrganDuty> GetDutyAsync(int id)
        {
            return await _dutyRepository.GetAsync(id);
        }
        public async Task<OrganDuty> GetDutyByGuidAsync(string guid)
        {
            return await _dutyRepository.GetByGuidAsync(guid);
        }
        public async Task<List<int>> GetDutyIdsAsync(int id)
        {
            return await _dutyRepository.GetIdsAsync(id);
        }
        public async Task<List<int>> GetDutyIdsByDepartmentIdAsync(int departmentId)
        {
            return await _dutyRepository.GetIdsByDepartmentIdAsync(departmentId);
        }
        public async Task<List<string>> GetDutyGuidsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return null;
            return await _dutyRepository.GetGuidsAsync(ids);
        }
    }
}
