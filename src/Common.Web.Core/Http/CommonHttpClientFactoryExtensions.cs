using Microsoft.Extensions.DependencyInjection;

namespace Common.Web.Core
{
    /// <summary>
    /// CommonHttpClientFactory Extensions
    /// </summary>
    public static class CommonHttpClientFactoryExtensions
    {
        /// <summary>
        /// Adds ICommonHttpClientFactory to the IServiceCollection
        /// </summary>
        public static IServiceCollection AddCommonHttpClientFactory(this IServiceCollection services)
        {
            services.AddSingleton<ICommonHttpClientFactory, CommonHttpClientFactory>();
            return services;
        }
    }
}