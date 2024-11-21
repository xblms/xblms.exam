using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class EditorTmWordOpenLayerController
    {
        [HttpGet, Route(Route)]
        public GetResult GetExample()
        {
            var tms = new StringBuilder();
            tms.AppendFormat(@"<p><strong>这是填空题，必须包含三个下划线，1+1=___?</strong></p>");
            tms.AppendFormat(@"<p>题目注释|填空题|A|1|1|填空题知识点|填空题题目解析</p>");

            //tms.AppendFormat(@"<p>题型|正确答案|知识点|难度|分数|题目解析</p>");

            tms.AppendFormat(@"<p></p>");

            tms.AppendFormat(@"<p><strong>这是单选题，1+1=?</strong></p>");
            tms.AppendFormat(@"<p>1</p>");
            tms.AppendFormat(@"<p>2</p>");
            tms.AppendFormat(@"<p>3</p>");
            tms.AppendFormat(@"<p>4</p>");
            tms.AppendFormat(@"<p>题目注释|单选题|A|1|1|单选题知识点|单选题题目解析</p>");

            tms.AppendFormat(@"<p></p>");

            tms.AppendFormat(@"<p><strong>这是多选题，1+1!=?</strong></p>");
            tms.AppendFormat(@"<p>1</p>");
            tms.AppendFormat(@"<p>2</p>");
            tms.AppendFormat(@"<p>3</p>");
            tms.AppendFormat(@"<p>4</p>");
            tms.AppendFormat(@"<p>题目注释|多选题|ACD|1|1|多选题知识点|多选题题目解析</p>");

            tms.AppendFormat(@"<p></p>");

            tms.AppendFormat(@"<p><strong>这是判断题，1+1=2?</strong></p>");
            tms.AppendFormat(@"<p>正确</p>");
            tms.AppendFormat(@"<p>错误</p>");
            tms.AppendFormat(@"<p>题目注释|判断题|A|1|1|判断题知识点|判断题题目解析</p>");

            tms.AppendFormat(@"<p></p>");

            tms.AppendFormat(@"<p><strong>这是简答题，请论证1+1=2。</strong></p>");
            tms.AppendFormat(@"<p>题目注释|简答题|因为1+1=2|1|1|简答题知识点|简答题题目解析</p>");

            tms.AppendFormat(@"<p></p>");

            tms.AppendFormat(@"<p><strong>这是一道有问题的题目。</strong></p>");
            tms.AppendFormat(@"<p>1</p>");
            tms.AppendFormat(@"<p>2</p>");
            tms.AppendFormat(@"<p>3</p>");
            tms.AppendFormat(@"<p>4</p>");
            tms.AppendFormat(@"<p>题目1注释|多选1题|E|1|1|多选题知识点|多选题题目解析|?</p>");

            tms.AppendFormat(@"<p></p>");

            return new GetResult
            {
                Example = tms.ToString(),
            };
        }
    }
}
