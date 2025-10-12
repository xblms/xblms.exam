using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using XBLMS.Configuration;
using XBLMS.Core.Utils;

namespace XBLMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InstallUtils.Init(AppDomain.CurrentDomain.BaseDirectory);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile(Constants.PackageFileName, optional: true, reloadOnChange: true)
                        .AddJsonFile(Constants.ConfigFileName, optional: true, reloadOnChange: true)
                        //.AddEnvironmentVariables(Constants.EnvironmentPrefix)
                        .AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options =>
                        {
                            options.Limits.MaxRequestBodySize = long.MaxValue;
                            options.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 1024, gracePeriod: TimeSpan.FromSeconds(10));
                        })
                        .UseIIS()
                        .UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                    loggerConfiguration.Enrich.FromLogContext();
                });
    }
}
