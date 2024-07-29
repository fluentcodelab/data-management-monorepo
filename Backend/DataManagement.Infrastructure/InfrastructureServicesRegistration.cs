using DataManagement.Application;
using DataManagement.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DataManagement.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static void AddDataManagementModule(this IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddDbContext<DataManagementDbContext>();
        services.AddScoped<IAdvisorRepository, AdvisorRepository>();
    }
}