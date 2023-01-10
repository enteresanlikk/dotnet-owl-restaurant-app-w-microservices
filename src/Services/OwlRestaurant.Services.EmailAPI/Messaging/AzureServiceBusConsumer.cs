using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.EmailAPI.Abstractions.Messaging;
using OwlRestaurant.Services.EmailAPI.Abstractions.Repositories;
using OwlRestaurant.Services.EmailAPI.Messages;
using OwlRestaurant.Services.EmailAPI.Models;
using OwlRestaurant.Services.EmailAPI.Repositories;
using System.Text;

namespace OwlRestaurant.Services.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly EmailRepository _emailRepository;
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;
    private readonly string _emailSubscriptionName;
    private readonly string _orderUpdatePaymentResultTopic;
    
    private readonly ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

    public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
    {
        _emailRepository = emailRepository;
        _configuration = configuration;

        _connectionString = _configuration["ServiceBus:ConnectionString"];
        _emailSubscriptionName = _configuration["ServiceBus:SubscriptionName"];
        _orderUpdatePaymentResultTopic = _configuration["ServiceBus:OrderUpdatePaymentResultTopic"];

        var client = new ServiceBusClient(_connectionString);
        
        _orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _emailSubscriptionName);
    }

    public async Task Start()
    {
        _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateRecieved;
        _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;

        await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
        await _orderUpdatePaymentStatusProcessor.DisposeAsync();
    }

    private async Task OnOrderPaymentUpdateRecieved(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        UpdatePaymentResultMessage updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

        await _emailRepository.SendAndLogEmail(updatePaymentResultMessage);
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }
}
