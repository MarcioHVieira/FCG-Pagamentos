using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Azure.Messaging.ServiceBus;
using Fcg.Common.Messaging.Abstractions;

namespace Pagamentos.Api.Infraestructures.Data
{
    public class PagamentoEventPublisher : IEventPublisher
    {
        private readonly string _provider;
        private readonly IConnection? _rabbitConnection;
        private readonly ServiceBusClient? _serviceBusClient;

        public PagamentoEventPublisher(
            string provider,
            IConnection? rabbitConnection,
            ServiceBusClient? serviceBusClient)
        {
            _provider = provider;
            _rabbitConnection = rabbitConnection;
            _serviceBusClient = serviceBusClient;
        }

        public async Task PublishAsync<T>(T evento, string queueName)
        {
            var payload = JsonSerializer.Serialize(evento, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            if (_provider == "RabbitMQ" && _rabbitConnection != null)
            {
                using var channel = _rabbitConnection.CreateModel();
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                var body = Encoding.UTF8.GetBytes(payload);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
            else if (_provider == "ServiceBus" && _serviceBusClient != null)
            {
                var sender = _serviceBusClient.CreateSender(queueName);
                var message = new ServiceBusMessage(payload);
                await sender.SendMessageAsync(message);
            }
            else
            {
                throw new InvalidOperationException("Provider de mensageria não configurado corretamente.");
            }
        }
    }
}