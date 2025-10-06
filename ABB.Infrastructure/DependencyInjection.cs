using ABB.Application.Common.Interfaces;
using ABB.Infrastructure.Data;
using ABB.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using Microsoft.EntityFrameworkCore;         // <-- tambahan
using Microsoft.Extensions.Configuration;  // <-- tambahan

namespace ABB.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddScoped(typeof(IDbConnection), typeof(ABBDbConnection));
            services.AddScoped(typeof(IDbConnectionCSM), typeof(ABBDbConnectionCSM));
            services.AddScoped(typeof(IDbConnectionPstNota), typeof(ABBDbConnectionPstNota));
            services.AddSingleton(typeof(ILog), typeof(Logger));


            // tambahan
            services.AddDbContext<ABBDbContextPstNota>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ABBConnectionPstNota")));
            services.AddScoped<IDbContextPstNota>(provider => provider.GetService<ABBDbContextPstNota>());
            // end tambahan

            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Helper"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            return services;
        }
    }
}