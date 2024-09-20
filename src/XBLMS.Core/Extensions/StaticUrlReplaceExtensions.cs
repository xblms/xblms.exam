using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Datory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XBLMS.Core.Services;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Extensions
{
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


            if (url.Contains("/home/"))
            {
                // 暂存原始响应内容
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    // 替换响应内容
                    context.Response.Body = responseBody;

                    await _next(context);

                    // 恢复原始响应内容
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    body = body.Replace("/sitefiles/", $"{hostUrl}/sitefiles/");
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    // 将修改后的响应内容写回原始响应流中
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(body));
                    context.Response.Body = originalBodyStream;
                }
            }
            else
            {
                await _next(context);
            }

        }
    }
}
