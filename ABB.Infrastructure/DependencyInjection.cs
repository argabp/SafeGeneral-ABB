using ABB.Application.Common.Interfaces;
using ABB.Infrastructure.Data;
using ABB.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace ABB.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDbConnection), typeof(ABBDbConnection));
            services.AddSingleton(typeof(ILog), typeof(Logger));
            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Helper"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            return services;
        }
    }
}