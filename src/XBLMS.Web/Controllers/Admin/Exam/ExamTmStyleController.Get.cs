using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmStyleController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var admin = await _authManager.GetAdminAsync();
 
            var list = await _tableStyleRepository.GetExamTmStylesAsync(true);
            if (list != null && list.Count > 0)
            {
                foreach (var style in list)
                {
                    style.Set("TypeName", style.InputType.GetDisplayName());
                }
            }

            return new GetResult
            {
                Styles = list,
                TableName = _examTmRepository.TableName,
                RelatedIdentities = _tableStyleRepository.EmptyRelatedIdentities
            };
        }
    }
}
