using System.Reflection;
using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using ABB.Infrastructure.Data;
using ABB.Infrastructure.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ABB.Api.Extensions
{
    public static class ServiceExtensions
    {
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

            services.AddHttpContextAccessor();

            //Read Configuration from appSettings by Serilog
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            services.AddControllers()
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}