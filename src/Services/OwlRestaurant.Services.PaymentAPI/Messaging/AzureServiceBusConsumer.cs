using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.PaymentAPI.Abstractions.Messaging;
using OwlRestaurant.Services.PaymentAPI.Messages;
using PaymentProcessor;
using System.Text;

namespace OwlRestaurant.Services.PaymentAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly IProcessPayment _processPayment;
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _messageBus;

    private readonly string _connectionString;
    
    private readonly string _paymentMessageTopic;
    private readonly string _orderUpdatePaymentResultTopic;
    
    private readonly string _paymentSubscriptionName;

    private readonly ServiceBusProcessor _paymentProcessor;

    public AzureServiceBusConsumer(IProcessPayment processPayment, IConfiguration configuration, IMessageBus messageBus)
    {
        _processPayment = processPayment;
        _configuration = configuration;
        _messageBus = messageBus;

        _connectionString = _configuration["ServiceBus:ConnectionString"];
        
        _paymentMessageTopic = _configuration["ServiceBus:PaymentMessageTopic"];
        _orderUpdatePaymentResultTopic = _configuration["ServiceBus:OrderUpdatePaymentResultTopic"];

        _paymentSubscriptionName = _configuration["ServiceBus:PaymentSubscriptionName"];

        var client = new ServiceBusClient(_connectionString);

        _paymentProcessor = client.CreateProcessor(_paymentMessageTopic, _paymentSubscriptionName);
    }

    public async Task Start()
    {
        _paymentProcessor.ProcessMessageAsync += OnProcessPayments;
        _paymentProcessor.ProcessErrorAsync += ErrorHandler;

        await _paymentProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _paymentProcessor.StopProcessingAsync();
        await _paymentProcessor.DisposeAsync();
    }

    private async Task OnProcessPayments(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

        var result = _processPayment.PaymentProcess();

        UpdatePaymentResultMessage updatePaymentResultMessage = new()
        {
            OrderId = paymentRequestMessage.OrderId,
            Status = result
        };

        try
        {
            await _messageBus.Publish(updatePaymentResultMessage, _orderUpdatePaymentResultTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }
}
