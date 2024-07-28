using DataManagement.Application.Contracts;
using DataManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DataManagement.Application;

public static class ApplicationServicesRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAdvisorService, AdvisorService>();
    }
}