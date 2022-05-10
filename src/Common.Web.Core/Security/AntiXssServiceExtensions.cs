using Microsoft.Extensions.DependencyInjection;

namespace Common.Web.Core
{
    /// <summary>
    /// SafeFile Download Service Extensions
    /// </summary>
    public static class AntiXssServiceExtensions
    {
        /// <summary>
        /// Adds IFileNameSanitizerService to IServiceCollection.
        /// </summary>
        public static IServiceCollection AddAntiXssService(this IServiceCollection services)
        {
            services.AddTransient<IAntiXssService, AntiXssService>();
            return services;
        }
    }
}