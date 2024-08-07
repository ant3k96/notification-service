using Mapster;
using Notification.Api.Model;
using Notification.Domain.Notifications;

namespace Notification.Api.Mapping
{
    public class NotificationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SendMessageRequest, NotificationMessage>()
                .Map(dest => dest.Sender, src => src.From)
                .Map(dest => dest.Receiver, src => src.To);
        }
    }
}
