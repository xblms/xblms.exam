using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesEditController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            var item = await _knowlegesRepository.GetAsync(request.Item.Id);

            item.Name = request.Item.Name;
            item.CoverImgUrl = request.Item.CoverImgUrl;
            await _knowlegesRepository.UpdateAsync(item);

            return new BoolResult { Value = true };
        }
    }
}
