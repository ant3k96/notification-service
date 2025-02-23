using MapsterMapper;
using Notification.Api.Model;
using Notification.Domain.Notifications;

namespace Notification.Api.UnitTests.Mapping
{
    public class NotificationMappingConfigTest
    {
        private readonly Mapper _mapper;

        public NotificationMappingConfigTest()
        {
            _mapper = MapsterConfig.GetMapper();
        }

        [Fact]
        public void SendMessageRequest_To_SmsNotification()
        {
            var sendMessageRequest = new SendMessageRequest
            {
                Body = "test",
                Sms = new PhoneSpecification
                {
                    From = "me",
                    To = "you "
                }
            };

            var result = _mapper.Map<NotificationSms>(sendMessageRequest);

            Assert.Equal(sendMessageRequest.Body, result.MessageBody);
            Assert.Equal(sendMessageRequest.Sms.From, result.Sender);
            Assert.Equal(sendMessageRequest.Sms.To, result.Receiver);
        }

        [Fact]
        public void SendMessageRequest_To_EmailNotification()
        {
            var sendMessageRequest = new SendMessageRequest
            {
                Body = "test",
                Email = new EmailSpecification
                {
                    From = "me",
                    To = "you",
                    Subject = "test"
                }
            };

            var result = _mapper.Map<NotificationEmail>(sendMessageRequest);

            Assert.Equal(sendMessageRequest.Body, result.MessageBody);
            Assert.Equal(sendMessageRequest.Email.From, result.Sender);
            Assert.Equal(sendMessageRequest.Email.To, result.Receiver);
            Assert.Equal(sendMessageRequest.Email.Subject, result.Subject);
        }
    }
}
