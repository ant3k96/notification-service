using Mapster;
using MapsterMapper;
using Notification.Api.Mapping;

namespace Notification.Api.UnitTests.Mapping
{
    internal class MapsterConfig
    {
        public static Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(NotificationMappingConfig).Assembly);

            return new Mapper(config);
        }
    }
}
