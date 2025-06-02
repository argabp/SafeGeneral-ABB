using System;
using System.IO;
using ABB.Application;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Domain.Models;
using ABB.Infrastructure;
using ABB.Infrastructure.Services;
using ABB.Web.Extensions;
using ABB.Web.Hubs;
using ABB.Web.Middleware;
using ABB.Web.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Quartz;

namespace ABB.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddCors( options =>
            {
                options.AddDefaultPolicy( policy =>
                {
                    // policy.SetIsOriginAllowed( origin => new Uri( origin ).IsLoopback );
                    // allow all origin 
                    policy.SetIsOriginAllowed( origin => true )
                        .AllowAnyHeader()
                        .WithMethods( "GET", "POST", "PATCH", "DELETE", "PUT" )
                        .AllowCredentials();
                } );
            } );
            
            var reportConfig = new ReportConfig();
            // Or bind directly from a custom JSON file:
            var configFilePath = Path.Combine(WebHostEnvironment.ContentRootPath, "Configs", "ReportConfig.json");
            var configJson = File.ReadAllText(configFilePath);
            reportConfig = JsonConvert.DeserializeObject<ReportConfig>(configJson);

            services.AddSingleton(reportConfig);
            
            services.AddSingleton(Configuration);
            services.AddSingleton(new ProgressBarDto());
            services.AddApplication();
            services.AddInfrastructure();
            services.AddSerializer();
            services.AddModulesFolder();
            services.AddPlugins(Configuration);
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddSchedulerHost();
            services.AddReports();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });
            // services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<ApplicationHub>();
            services.Configure<IISServerOptions>(options => { options.MaxRequestBodySize = int.MaxValue; });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IInitialService initial)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<HeaderMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseSession();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UserStaticFilesModulesFolder();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapHub<ApplicationHub>("/applicationHub");
            });
            initial.Execute().GetAwaiter().GetResult();
        }
    }
}