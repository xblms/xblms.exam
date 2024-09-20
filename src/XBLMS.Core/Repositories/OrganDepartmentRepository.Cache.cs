using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using NPOI.POIFS.Properties;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDepartmentRepository
    {
        public async Task<List<OrganDepartment>> GetListAsync()
        {
            return await GetAllAsync();
        }
        public async Task<List<OrganDepartment>> GetListAsync(int companyId)
        {
            var list = await GetAllAsync();
            return list.Where(d => d.CompanyId == companyId).ToList();
        }
        public async Task<List<OrganDepartment>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(OrganDepartment.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }
        public async Task<OrganDepartment> GetAsync(int companyId,string name)
        {
            var departments = await GetAllAsync();
            var list = departments.ToList();
            return list.Where(x => x.Name == name && x.CompanyId==companyId).FirstOrDefault();
        }
        public async Task<OrganDepartment> GetAsync(int id)
        {
            var departments = await GetAllAsync();
            var list = departments.ToList();
            return list.Where(x => x.Id == id).FirstOrDefault();
        }
        public async Task<OrganDepartment> GetByGuidAsync(string guid)
        {
            var departments = await GetAllAsync();
            var list = departments.ToList();
            return list.Where(x => x.Guid == guid).FirstOrDefault();
        }
        public async Task<List<int>> GetIdsAsync(int id)
        {
            var all = await GetAllAsync();
            var ids = new List<int>();
            ids.Add(id);
            await GetIdsAsync(ids, all, id);
            return ids;
        }

        public async Task<List<string>> GetGuidsAsync(List<int> ids)
        {
            var all = await GetAllAsync();
            var selectList = all.Where(d => ids.Contains(d.Id));

            return selectList.Select(d => d.Guid).ToList();
        }
        private async Task GetIdsAsync(List<int> ids,List<OrganDepartment> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }
        public async Task<List<int>> GetIdsByCompanyIdAsync(int companyId)
        {
            var ids = new List<int>();

            var departments = await GetAllAsync();
            var all = departments.ToList();
            var children = all.Where(x => x.CompanyId == companyId);
            foreach (var child in children)
            {
                ids.Add(child.Id);
            }
            return ids;
        }

        public async Task<List<OrganDepartment>> GetPathNamesAsync(int id)
        {
            var result= new List<OrganDepartment>();
            var info = await GetAsync(id);
            if (info != null)
            {
                result.Add(info);
                await GetPathNamesAsync(result, info.ParentId);
            }
            result= result.OrderBy(d => d.Id).ToList();
            return result;
        }
        public async Task GetPathNamesAsync(List<OrganDepartment> names,int parentId)
        {
            if (parentId > 0)
            {
                var info=await GetAsync(parentId);
                names.Add(info);
                await GetPathNamesAsync(names, info.ParentId);
            }
        }
    }
}
