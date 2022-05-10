using Microsoft.Extensions.DependencyInjection;

namespace Common.Web.Core
{
    /// <summary>
    /// EnhancedStackTraceService Extensions
    /// </summary>
    public static class EnhancedStackTraceServiceExtensions
    {
        /// <summary>
        /// Adds IEnhancedStackTraceService to IServiceCollection.
        /// </summary>
        public static IServiceCollection AddEnhancedStackTraceService(this IServiceCollection services)
        {
            services.AddSingleton<IEnhancedStackTraceService, EnhancedStackTraceService>();
            return services;
        }
    }
}