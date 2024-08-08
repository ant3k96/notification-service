using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Notification.Api.Controllers.Hanlders;
using Notification.Api.Exceptions;
using Notification.Api.Model;
using Notification.Api.Options;
using Notification.Domain.Notifications;
using Notification.Services;

namespace Notification.Api.UnitTests
{
    public class EmailNotificationHandlerTests
    {
        private readonly Mock<IMessageProvider> _mockMessageProvider1;
        private readonly Mock<IMessageProvider> _mockMessageProvider2;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IOptions<EmailChannelOptions>> _mockOptions;
        private readonly Mock<ILogger<EmailNotificationHandler>> _mockLogger;
        private readonly EmailChannelOptions _channelOptions;
        private readonly EmailNotificationHandler _handler;

        public EmailNotificationHandlerTests()
        {
            _mockMessageProvider1 = new Mock<IMessageProvider>();
            _mockMessageProvider2 = new Mock<IMessageProvider>();
            var messageProviders = new List<IMessageProvider>
            {
            _mockMessageProvider1.Object,
            _mockMessageProvider2.Object
            };

            _mockMapper = new Mock<IMapper>();
            _mockOptions = new Mock<IOptions<EmailChannelOptions>>();
            _mockLogger = new Mock<ILogger<EmailNotificationHandler>>();
            _channelOptions = new EmailChannelOptions { Enabled = true };

            _mockOptions.Setup(o => o.Value).Returns(_channelOptions);
            _handler = new EmailNotificationHandler(
                messageProviders,
                _mockMapper.Object,
                _mockOptions.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ChannelDisabled_LogsDebugMessage()
        {
            // Arrange
            _channelOptions.Enabled = false;
            var request = new SendMessageRequest
            {
                From = "test",
                Email = "test",
                PhoneNumber = "test1",
                Body = "test2"
            };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Email Notification channel disabled")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenChannelEnabledAndMessageProvidersWorks_MapsAndSendsNotification()
        {
            // Arrange
            var request = new SendMessageRequest
            {
                From = "test",
                Email = "test",
                PhoneNumber = "test1",
                Body = "test2"
            };
            var notification = new NotificationEmail();

            _mockMapper.Setup(m => m.Map<NotificationEmail>(It.IsAny<SendMessageRequest>())).Returns(notification);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<NotificationEmail>(request), Times.Once);
            _mockMessageProvider1.Verify(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()), Times.Once);
            _mockMessageProvider2.Verify(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()), Times.Never);
        }

        [Fact]
        public async Task Handle_SendEmailThrowsException_LogsErrorAndRetries()
        {
            // Arrange
            var request = new SendMessageRequest
            {
                From = "test",
                Email = "test",
                PhoneNumber = "test1",
                Body = "test2"
            };
            var notification = new NotificationEmail();

            _mockMessageProvider1.Setup(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()))
                .Throws(new Exception("Service unavailable"));

            _mockMessageProvider2.Setup(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map<NotificationEmail>(It.IsAny<SendMessageRequest>())).Returns(notification);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<NotificationEmail>(request), Times.Once);
            _mockMessageProvider1.Verify(p => p.SendEmailAsync(notification), Times.Once);
            _mockMessageProvider2.Verify(p => p.SendEmailAsync(notification), Times.Once);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error while sending email")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_AllMessageProvidersFail_ThrowsServiceUnavailableException()
        {
            // Arrange
            var request = new SendMessageRequest
            {
                From = "test",
                Email = "test",
                PhoneNumber = "test1",
                Body = "test2"
            };
            var notification = new NotificationEmail();

            _mockMessageProvider1.Setup(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()))
                .Throws(new Exception("Service unavailable"));
            _mockMessageProvider2.Setup(p => p.SendEmailAsync(It.IsAny<NotificationEmail>()))
                .Throws(new Exception("Service unavailable"));

            _mockMapper.Setup(m => m.Map<NotificationEmail>(It.IsAny<SendMessageRequest>())).Returns(notification);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceUnavailableException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_EmailNotProvided_ThrowsInvalidMessageDataException()
        {
            // Arrange
            var request = new SendMessageRequest
            {
                From = "test",
                Email = "",
                PhoneNumber = "test1",
                Body = "test2"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidMessageDataException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}