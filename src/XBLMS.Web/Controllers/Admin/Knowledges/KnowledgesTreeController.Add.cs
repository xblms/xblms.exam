using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesTreeController
    {
        [HttpPost, Route(RouteAdd)]
        public async Task<ActionResult<BoolResult>> Add([FromBody] GetTreeNamesRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var insertedTreeIdHashtable = new Hashtable { [1] = request.ParentId };
            var names = request.Names.Split('\n');
            foreach (var item in names)
            {
                if (string.IsNullOrEmpty(item)) continue;

                var count = StringUtils.GetStartCount("－", item) == 0 ? StringUtils.GetStartCount("-", item) : StringUtils.GetStartCount("－", item);
                var name = item.Substring(count, item.Length - count);
                count++;

                if (!string.IsNullOrEmpty(name) && insertedTreeIdHashtable.Contains(count))
                {
                    if (name.Contains('(') && name.Contains(')'))
                    {
                        var length = name.IndexOf(')') - name.IndexOf('(');
                        if (length > 0)
                        {
                            name = name.Substring(0, name.IndexOf('('));
                        }
                    }
                    name = name.Trim();

                    var parentId = (int)insertedTreeIdHashtable[count];

                    var insertedId = await _knowlegesTreeRepository.InsertAsync(new KnowledgesTree
                    {
                        Name = name,
                        ParentId = parentId,
                        CompanyId = admin.CompanyId,
                        DepartmentId = admin.DepartmentId,
                        CreatorId = admin.Id,
                        DepartmentParentPath = admin.DepartmentParentPath,
                        CompanyParentPath = admin.CompanyParentPath,
                    });

                    var insertTree = await _knowlegesTreeRepository.GetAsync(insertedId);
                    insertTree.ParentPath = await _knowlegesTreeRepository.GetParentPathAsync(insertTree.Id);
                    await _knowlegesTreeRepository.UpdateAsync(insertTree);

                    await _authManager.AddAdminLogAsync("新增知识库分类", $"{name}");


                    insertedTreeIdHashtable[count + 1] = insertedId;
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
