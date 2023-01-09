using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace OwlRestaurant.Integration.MessageBus;

public class AzureServiceMessageBus : IMessageBus
{
    private readonly string _connectionString;

    public AzureServiceMessageBus(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Publish(BaseMessage message, string topicName)
    {
        await using var client = new ServiceBusClient(_connectionString);

        ServiceBusSender sender = client.CreateSender(topicName);

        string messageBody = JsonConvert.SerializeObject(message);
        ServiceBusMessage busMessage = new ServiceBusMessage(messageBody)
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        await sender.SendMessageAsync(busMessage);

        await client.DisposeAsync();
    }
}