using Demo_Api.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Demo_API.Infrastructure.Services;

namespace Demo_API.Infrastructure
{
    /// <summary>
    /// Dependency injection
    /// </summary>
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IHttpClientHandler, Services.Handlers.HttpClientHandler>();
            services.AddTransient<ITargetAssetService, TargetAssetService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<ITargetAssetRules, TargetAssetRules>();

            return services;
        }
    }
}
