namespace OwlRestaurant.Services.OrderAPI.Abstractions.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}
