using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
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

                    var insertedTkId = await _examTmTreeRepository.InsertAsync(new ExamTmTree
                    {
                        Name = name,
                        ParentId = parentId,
                        CompanyId=admin.CompanyId,
                        DepartmentId= admin.DepartmentId,
                        CreatorId= admin.CreatorId
                    });

                    await _authManager.AddAdminLogAsync("新增题目分类", $"{name}");


                    insertedTreeIdHashtable[count + 1] = insertedTkId;
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
