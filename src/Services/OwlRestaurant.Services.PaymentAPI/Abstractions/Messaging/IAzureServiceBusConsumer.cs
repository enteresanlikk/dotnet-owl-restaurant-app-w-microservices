namespace OwlRestaurant.Services.PaymentAPI.Abstractions.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}
