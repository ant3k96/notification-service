using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace NotificationApi.DependencyInjection
{
    public static class MapsterRegistration
    {
        public static IServiceCollection AddMappingServices(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.TryAddSingleton<IMapper, Mapper>();
            return services;
        }
    }
}
