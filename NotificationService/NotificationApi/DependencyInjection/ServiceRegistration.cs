using FluentValidation;
using MediatR;
using Notification.Api.Controllers.Handlers;
using Notification.Api.Controllers.Validation;
using Notification.Api.Model;
using Notification.Api.Options;
using Notification.Services.AmazonSns;
using Notification.Services.Interfaces;
using Notification.Services.Options;
using Notification.Services.Twilio;
using Scrutor;

namespace NotificationApi.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var amazonSnsEmailOptions = GetOptions<AmazonSnsEmailServiceOptions>(services, configuration, AmazonSnsEmailServiceOptions.SectionName);
            var amazonSnsSmsOptions = GetOptions<AmazonSnsSmsServiceOptions>(services, configuration, AmazonSnsSmsServiceOptions.SectionName);

            var twilioSmsOptions = GetOptions<TwilioSmsServiceOptions>(services, configuration, TwilioSmsServiceOptions.SectionName);
            var twilioEmailOptions = GetOptions<TwilioSendGridEmailOptions>(services, configuration, TwilioSendGridEmailOptions.SectionName);

            _ = GetOptions<EmailChannelOptions>(services, configuration, EmailChannelOptions.SectionName);
            _ = GetOptions<SmsChannelOptions>(services, configuration, SmsChannelOptions.SectionName);


            if (amazonSnsEmailOptions.Enabled)
                services.AddSingleton<IEmailProvider, AmazonSnsEmailMockService>();

            if (amazonSnsSmsOptions.Enabled)
                services.AddSingleton<ISmsProvider, AmazonSnsSmsMockService>();

            if (twilioSmsOptions.Enabled)
                services.AddSingleton<ISmsProvider, TwilioSmsMockService>();

            if (twilioEmailOptions.Enabled)
                services.AddSingleton<IEmailProvider, TwilioSendGridEmailMockService>();

            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(Program))
                    .RegisterHandlers(typeof(INotificationHandler<>));
            });

            services.Decorate(typeof(INotificationHandler<>), typeof(RetryDecorator<>));

            services.AddScoped<IValidator<SendMessageRequest>, MessageRequestValidator>();

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
