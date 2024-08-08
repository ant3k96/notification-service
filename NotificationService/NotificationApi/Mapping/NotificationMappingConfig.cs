using Mapster;
using Notification.Api.Model;
using Notification.Domain.Notifications;

namespace Notification.Api.Mapping
{
    public class NotificationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SendMessageRequest, NotificationSms>()
                .Map(dest => dest.Sender, src => src.From)
                .Map(dest => dest.Receiver, src => src.PhoneNumber)
                .Map(dest => dest.MessageBody, src => src.Body);

            config.NewConfig<SendMessageRequest, NotificationEmail>()
                .Map(dest => dest.Sender, src => src.From)
                .Map(dest => dest.Receiver, src => src.Email)
                .Map(dest => dest.MessageBody, src => src.Body);
        }
    }
}
