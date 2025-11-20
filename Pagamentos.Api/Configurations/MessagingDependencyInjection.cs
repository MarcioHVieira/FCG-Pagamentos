using Fcg.Common.Messaging.RabbitMQ;
using Fcg.Common.Messaging.ServiceBus;
using RabbitMQ.Client;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Pagamentos.Api.Infraestructures.Data;
using Fcg.Common.Messaging.Abstractions;

namespace Pagamentos.Api.Configurations
{
    public static class MessagingDependencyInjection
    {
        public static void RegisterPublisher(WebApplicationBuilder builder, string messagingProvider)
        {
            if (string.IsNullOrWhiteSpace(messagingProvider))
                throw new InvalidOperationException("Messaging:Provider não configurado.");

            if (messagingProvider == "RabbitMQ")
            {
                builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

                builder.Services.AddSingleton<IConnection>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                    var factory = new ConnectionFactory()
                    {
                        HostName = settings.HostName,
                        UserName = settings.UserName,
                        Password = settings.Password
                    };
                    return factory.CreateConnection();
                });

                builder.Services.AddSingleton<PagamentoEventPublisher>(sp =>
                {
                    var connection = sp.GetRequiredService<IConnection>();
                    return new PagamentoEventPublisher("RabbitMQ", connection, null);
                });
            }
            else if (messagingProvider == "ServiceBus")
            {
                builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));

                builder.Services.AddSingleton(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                    return new ServiceBusClient(settings.ConnectionString);
                });

                builder.Services.AddSingleton<PagamentoEventPublisher>(sp =>
                {
                    var client = sp.GetRequiredService<ServiceBusClient>();
                    return new PagamentoEventPublisher("ServiceBus", null, client);
                });
            }

            builder.Services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<PagamentoEventPublisher>());
        }
    }
}