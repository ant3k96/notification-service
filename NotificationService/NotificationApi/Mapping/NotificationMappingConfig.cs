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
                .Map(dest => dest.Sender, src => src.Phone.From)
                .Map(dest => dest.Receiver, src => src.Phone.To)
                .Map(dest => dest.MessageBody, src => src.Body);

            config.NewConfig<SendMessageRequest, NotificationEmail>()
                .Map(dest => dest.Sender, src => src.Email.From)
                .Map(dest => dest.Receiver, src => src.Email.To)
                .Map(dest => dest.MessageBody, src => src.Body)
                .Map(dest => dest.Subject, src => src.Email.Subject);
        }
    }
}
