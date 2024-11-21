using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperInfoController
    {
        [HttpPost, Route(RouteClientExam)]
        public StringResult ClientExam([FromBody] GetClientUrlRequest request)
        {
            var host = Request.Host.Port.HasValue ? $"{Request.Host.Host}:{Request.Host.Port}" : Request.Host.Host;
            var hostUrl = Request.Scheme + "://" + PageUtils.Combine(host, $"home/entryExamClient?token={request.Token}&id={request.Id}");

            //var hostUrl = $"https://localhost:55212/home/entryExamClient?token={request.Token}&id={request.Id}";

            hostUrl = TranslateUtils.EncryptStringBySecretKey(hostUrl, "kaoshiduanclient");

            return new StringResult { Value = hostUrl };

        }
        [HttpGet, Route(RouteClientExamStatus)]
        public GetCheckResult ClientExamGetStatus()
        {
            return new GetCheckResult
            {
                Success = true,
                Msg = ""
            };
            //var rkey = Registry.ClassesRoot;
            //var ss = rkey.GetSubKeyNames().Contains("KaoshiduanExamClient");
            //var msg = "";
            //if (!ss)
            //{
            //    msg = "请先下载并安装客户端工具";
            //}
            //return new GetCheckResult
            //{
            //    Success = ss,
            //    Msg = msg
            //};
        }
    }
}
