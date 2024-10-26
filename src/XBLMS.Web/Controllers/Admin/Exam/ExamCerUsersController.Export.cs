using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerUsersController
    {
        [HttpGet, Route(RouteExport)]
        public async Task<ActionResult<StringResult>> Export([FromQuery] GetUserRequest request)
        {
            var (total, list) = await _examCerUserRepository.GetListAsync(request.Id, request.Keywords, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            var cer = await _examCerRepository.GetAsync(request.Id);

            var fileName = $"{cer.Name}-获证人员列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "序号",
                "账号",
                "姓名",
                "组织",
                "试卷",
                "开考时间",
                "交卷时间",
                "用时",
                "客观题成绩",
                "主观题成绩",
                "成绩",
                "证书名称",
                "证书编码",
                "颁发单位",
                "获证时间",
            };
            var rows = new List<List<string>>();

            if (total > 0)
            {
                var index = 1;
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    var start = await _examPaperStartRepository.GetAsync(item.ExamStartId);

                    rows.Add(new List<string>
                    {
                        index.ToString(),
                        user.UserName,
                        user.DisplayName,
                        user.Get("OrganNames").ToString(),
                        paper.Title,
                        start.BeginDateTime.Value.ToString(),
                        start.EndDateTime.Value.ToString(),
                        DateUtils.SecondToHms(start.ExamTimeSeconds),
                        start.ObjectiveScore.ToString(),
                        start.SubjectiveScore.ToString(),
                        start.Score.ToString(),
                        cer.Name,
                        item.CerNumber,
                        cer.OrganName,
                        item.CerDateTime.Value.ToString()
                    });
                    index++;
                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
