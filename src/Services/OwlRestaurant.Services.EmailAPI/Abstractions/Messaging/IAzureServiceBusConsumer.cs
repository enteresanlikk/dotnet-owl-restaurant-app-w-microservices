namespace OwlRestaurant.Services.EmailAPI.Abstractions.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}
