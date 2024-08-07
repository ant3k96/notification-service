using MediatR;
using Notification.Api.Controllers.Hanlders;
using Notification.Api.Options;
using Notification.Services;
using Notification.Services.AmazonSns;
using Notification.Services.Options;
using Notification.Services.Twilio;
using Scrutor;

namespace NotificationApi.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var amazonSnsOptions = GetOptions<AmazonSnsMessageServiceOptions>(services, configuration, AmazonSnsMessageServiceOptions.SectionName);
            var twilioOptions = GetOptions<TwilioMessageServiceOptions>(services, configuration, TwilioMessageServiceOptions.SectionName);

            _ = GetOptions<EmailChannelOptions>(services, configuration, EmailChannelOptions.SectionName);
            _ = GetOptions<SmsChannelOptions>(services, configuration, SmsChannelOptions.SectionName);


            if (amazonSnsOptions.Enabled)
            {
                services.AddSingleton<IMessageProvider, AmazonSnsMessageService>();
            }
            if (twilioOptions.Enabled)
            {
                services.AddSingleton<IMessageProvider, TwilioMessageService>();
            }

            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(Program))
                    .RegisterHandlers(typeof(INotificationHandler<>));
            });

            services.Decorate(typeof(INotificationHandler<>), typeof(RetryDecorator<>));

            services.AddMappingServices();

            return services;
        }

        private static T GetOptions<T>(IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
        {
            var configurationSection = configuration.GetRequiredSection(sectionName);
            services.Configure<T>(configurationSection);

            return configurationSection.Get<T>()!;
        }

        private static IImplementationTypeSelector RegisterHandlers(this IImplementationTypeSelector selector, Type type)
        {
            return selector.AddClasses(c =>
                    c.AssignableTo(type)
                        .Where(t => t != typeof(RetryDecorator<>))
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        }
    }
}
