using Microsoft.Extensions.DependencyInjection;

namespace Common.Web.Core
{
    /// <summary>
    /// SerializationProvider Extensions
    /// </summary>
    public static class SerializationProviderExtensions
    {
        /// <summary>
        /// Adds ISerializationProvider to IServiceCollection.
        /// </summary>
        public static IServiceCollection AddSerializationProvider(this IServiceCollection services)
        {
            services.AddSingleton<ISerializationProvider, SerializationProvider>();
            return services;
        }
    }
}