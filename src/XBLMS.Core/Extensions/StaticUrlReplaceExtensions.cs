using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace XBLMS.Core.Extensions
{
    public class TextReplaceFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            // 判断响应是否为200
            if (resultContext.HttpContext.Response.StatusCode == 200)
            {
                // 暂存原始响应内容
                var originalBodyStream = resultContext.HttpContext.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    // 替换响应内容
                    resultContext.HttpContext.Response.Body = responseBody;

                    // 读取响应内容并替换
                    var body = await new StreamReader(originalBodyStream).ReadToEndAsync();
                    body = body.Replace("oldText", "newText");

                    // 将修改后的响应内容写回原始响应流中
                    await resultContext.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(body));
                    resultContext.HttpContext.Response.Body = originalBodyStream;
                }
            }
        }
    }

    public static class ReplaceContentMiddlewareExtensions
    {
        public static IApplicationBuilder UseReplaceContentMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StaticUrlReplaceExtensions>();
        }
    }

    public class StaticUrlReplaceExtensions
    {
        private readonly RequestDelegate _next;

        public StaticUrlReplaceExtensions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var url = context.Request.Path.Value;
            var host = context.Request.Host.Port.HasValue ? $"{context.Request.Host.Host}:{context.Request.Host.Port}" : context.Request.Host.Host;
            var hostUrl = context.Request.Scheme + "://" + host;


            var originalBody = context.Response.Body;
            var newResponseContent = "";

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memoryStream))
                    {
                        newResponseContent = await reader.ReadToEndAsync();
                        //newResponseContent = newResponseContent.Replace("OldString", "NewString");
                        newResponseContent = newResponseContent.Replace("/sitefiles/upload/", $"{hostUrl}/sitefiles/upload/");
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var writer = new StreamWriter(memoryStream))
                    {
                        await writer.WriteAsync(newResponseContent);
                        await writer.FlushAsync();
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }

            //if (url.Contains("/home/"))
            //{
            //    // 暂存原始响应内容
            //    var originalBodyStream = context.Response.Body;
            //    using (var responseBody = new MemoryStream())
            //    {
            //        // 替换响应内容
            //        context.Response.Body = responseBody;

            //        await _next(context);

            //        // 恢复原始响应内容
            //        context.Response.Body.Seek(0, SeekOrigin.Begin);
            //        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            //        body = body.Replace("/sitefiles/upload/", $"{hostUrl}/sitefiles/upload/");
            //        context.Response.Body.Seek(0, SeekOrigin.Begin);

            //        // 将修改后的响应内容写回原始响应流中
            //        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(body));
            //        context.Response.Body = originalBodyStream;
            //    }
            //}
            //else
            //{
            //    await _next(context);
            //}

        }
    }
}
