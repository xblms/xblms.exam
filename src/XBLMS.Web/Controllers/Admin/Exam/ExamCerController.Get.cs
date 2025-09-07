﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest reqeust)
        {
            var list = await _examCerRepository.GetListAsync();
            if (!string.IsNullOrEmpty(reqeust.Title))
            {
                list = list.FindAll(cer => cer.Name.Contains(reqeust.Title)).ToList();
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Set("PaperCount", await _examPaperRepository.GetCerCount(item.Id));
                    item.Set("UserCount", await _examCerUserRepository.GetCountAsync(item.Id));
                }
            }
            return new GetResult
            {
                Items = list,
            };
        }
    }
}
