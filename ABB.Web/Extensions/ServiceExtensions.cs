using System;
using System.IO;
using System.Reflection;
using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using ABB.Infrastructure.Data;
using ABB.Infrastructure.Validators;
using ABB.Web.JobSchedules;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Quartz;
using Serilog;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.WebReportDesigner.Services;

namespace ABB.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void AddSerializer(this IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddNewtonsoftJson(o =>
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public static void AddModulesFolder(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleLocationExpander());
            });
        }

        public static void AddReports(this IServiceCollection services)
        {
            services.Configure<IISServerOptions>(opt => opt.AllowSynchronousIO = true);
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.TryAddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
            {
                ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
                HostAppId = "AbbSafeGeneral",
                Storage = new FileStorage("C:\\TelerikTemp"),
                ReportSourceResolver = new TypeReportSourceResolver().AddFallbackResolver
                    (new UriReportSourceResolver(GetReportsDir(sp)))
            });

            services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            {
                DefinitionStorage = new FileDefinitionStorage(GetReportsDir(sp)),
                SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting")),
                ResourceStorage = new ResourceStorage(
                    Path.Combine(GetReportsDir(sp), "Resources"))
            });
        }

        private static string GetReportsDir(IServiceProvider sp)
        {
            return Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Modules", "Reports",
                "ReportFiles");
        }

        static IConfiguration ResolveSpecificReportingConfiguration(IWebHostEnvironment environment)
        {
            var reportingConfigFileName = Path.Combine(environment.ContentRootPath, "appsettings.json");
            return new ConfigurationBuilder()
                .AddJsonFile(reportingConfigFileName, true)
                .Build();
        }

        public static void AddPlugins(this IServiceCollection services
            , IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ABBConnection");
            services.AddScoped<IRoleValidator<AppRole>, CustomRoleValidator>();
            services.AddDbContext<ABBDbContext>(c => c.UseSqlServer(connectionString));
            services.AddIdentity<AppUser, AppRole>(option => { option.Lockout.AllowedForNewUsers = false; })
                .AddRoleValidator<CustomRoleValidator>()
                .AddEntityFrameworkStores<ABBDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped(typeof(IDbContext), typeof(ABBDbContext));
            
            string connectionStringCSM = configuration.GetConnectionString("ABBConnectionCSM");
            services.AddDbContext<ABBDbContextCSM>(c => c.UseSqlServer(connectionStringCSM));
            services.AddScoped(typeof(IDbContextCSM), typeof(ABBDbContextCSM));
            
            string connectionStringPstNota = configuration.GetConnectionString("ABBConnectionPstNota");
            services.AddDbContext<ABBDbContextPstNota>(c => c.UseSqlServer(connectionStringPstNota));
            services.AddScoped(typeof(ABBDbContextPstNota), typeof(ABBDbContextPstNota));
            
            services.AddSingleton<IDbContextFactory, DbContextFactory>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddHttpContextAccessor();

            services.AddKendo();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            //Read Configuration from appSettings by Serilog
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        
        public static void AddSchedulerHost(this IServiceCollection services)
        {
            // Add the required Quartz.NET services
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionJobFactory();
                // Create a "key" for the job
                var jobKey = new JobKey("ReportCleanUpJob.cs");

                // Register the job with the DI container
                q.AddJob<ReportCleanUpJob>(opts => opts.WithIdentity(jobKey));

                // Create a trigger for the job
                q.AddTrigger(opts => opts
                    .ForJob(jobKey) // link to the HelloWorldJob
                    .StartAt(DateBuilder.TodayAt(00, 00, 00)) // Start at midnight
                    .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())); // Repeat every 24 hours
            });

            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
        }
    }
}