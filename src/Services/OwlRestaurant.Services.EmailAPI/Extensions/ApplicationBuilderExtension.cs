using OwlRestaurant.Services.EmailAPI.Abstractions.Messaging;

namespace OwlRestaurant.Services.EmailAPI.Extensions;

public static class ApplicationBuilderExtension
{
    public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        var hostedApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostedApplicationLife.ApplicationStarted.Register(OnStart);
        hostedApplicationLife.ApplicationStopped.Register(OnStop);

        return app;
    }

    public static void OnStart()
    {
        ServiceBusConsumer.Start();
    }

    public static void OnStop()
    {
        ServiceBusConsumer.Stop();
    }
}
