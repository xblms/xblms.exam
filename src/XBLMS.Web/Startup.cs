using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XBLMS.Core.Extensions;
using XBLMS.Core.Services;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web
{
    public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public Startup(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var directory = new DirectoryInfo(_env.ContentRootPath);
            services.AddDataProtection().PersistKeysToFileSystem(directory);

            var entryAssembly = Assembly.GetExecutingAssembly();
            var assemblies = new List<Assembly> { entryAssembly }.Concat(entryAssembly.GetReferencedAssemblies().Select(Assembly.Load));

            var settingsManager = services.AddSettingsManager(_config, _env.ContentRootPath, _env.WebRootPath, entryAssembly);

            if (settingsManager.CorsIsOrigins)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(CorsPolicy,
                        builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins(settingsManager.CorsOrigins)
                            .AllowCredentials()
                    );
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(CorsPolicy,
                        builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(x => true)
                            .AllowCredentials()
                    );
                });
            }

            services.AddHttpContextAccessor();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(StringUtils.GetSecurityKeyBytes(settingsManager.SecurityKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        if (!context.Request.Query.TryGetValue("access_token", out var values))
                        {
                            return Task.CompletedTask;
                        }
                        if (values.Count > 1)
                        {
                            return Task.CompletedTask;
                        }
                        var token = values.Single();
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            return Task.CompletedTask;
                        }
                        context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<FormOptions>(x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
            });

            services.AddHealthChecks();

            services.AddRazorPages();

            services.AddCache(settingsManager.Redis.ConnectionString);

            services.AddTaskServices();
            services.AddRepositories(assemblies);
            services.AddServices();
      

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services
                .AddControllers()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ContractResolver
                        = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "zh-CN", uiCulture: "zh-CN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });


            if (!settingsManager.IsSafeMode)
            {
                //http://localhost:5000/api/swagger/v1/swagger.json
                //http://localhost:5000/api/swagger/
                //http://localhost:5000/api/docs/
                services.AddOpenApiDocument(config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "XBLMS API";
                        document.Info.Description = "XBLMS API";
                        document.Info.Contact = new NSwag.OpenApiContact
                        {
                            Name = "XBLMS",
                            Email = string.Empty,
                            Url = string.Empty
                        };
                    };
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISettingsManager settingsManager, IErrorLogRepository errorLogRepository)
        {
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                try
                {
                    errorLogRepository.AddErrorLogAsync(exception).GetAwaiter().GetResult();
                }
                catch
                {
                    // ignored
                }

                string result;
                if (env.IsDevelopment())
                {
                    result = TranslateUtils.JsonSerialize(new
                    {
                        exception.Message,
                        exception.StackTrace,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    result = TranslateUtils.JsonSerialize(new
                    {
                        exception.Message,
                        CreatedDate = DateTime.Now
                    });
                }
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseCors(CorsPolicy);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/404.html";
                    await next();
                }

            });

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".properties"] = "application/octet-stream";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //app.UseStaticFiles();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("zh-CN")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

          

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<SignalRHubManager>("/xblmspkroomsocket");
            });

            app.UseRequestLocalization();


            if (!settingsManager.IsSafeMode)
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
                app.UseReDoc(settings =>
                {
                    settings.Path = "/api/docs";
                    settings.DocumentPath = "/swagger/v1/swagger.json";
                });
            }

    
        }
    }
}
