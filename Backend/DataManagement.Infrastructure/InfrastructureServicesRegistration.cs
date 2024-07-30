using DataManagement.Application;
using DataManagement.Application.Contracts;
using DataManagement.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace DataManagement.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static void AddDataManagement(this IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddDbContext<DataManagementDbContext>();
        services.AddScoped<IAdvisorRepository, AdvisorRepository>();
        services.AddSingleton<ICacheService, MRUCacheService>();
    }
}