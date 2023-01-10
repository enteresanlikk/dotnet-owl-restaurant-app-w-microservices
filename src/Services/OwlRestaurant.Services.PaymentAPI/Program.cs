using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.PaymentAPI.Abstractions.Messaging;
using OwlRestaurant.Services.PaymentAPI.Extensions;
using OwlRestaurant.Services.PaymentAPI.Messaging;
using PaymentProcessor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProcessPayment, ProcessPayment>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus>(m => new AzureServiceMessageBus(builder.Configuration["ServiceBus:ConnectionString"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAzureServiceBusConsumer();

app.Run();
