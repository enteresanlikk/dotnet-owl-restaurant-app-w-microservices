using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.OrderAPI.Abstractions.Messaging;
using OwlRestaurant.Services.OrderAPI.Abstractions.Repositories;
using OwlRestaurant.Services.OrderAPI.Messages;
using OwlRestaurant.Services.OrderAPI.Models;
using OwlRestaurant.Services.OrderAPI.Repositories;
using System.Text;

namespace OwlRestaurant.Services.OrderAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly OrderRepository _orderRepository;
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _messageBus;

    private readonly string _connectionString;
    private readonly string _checkoutMesageTopic;
    private readonly string _paymentMessageTopic;
    private readonly string _orderUpdatePaymentResultTopic;
    private readonly string _checkoutSubscriptionName;

    private readonly ServiceBusProcessor _checkoutProcessor;
    private readonly ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

    public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
    {
        _orderRepository = orderRepository;
        _configuration = configuration;
        _messageBus = messageBus;

        _connectionString = _configuration["ServiceBus:ConnectionString"];

        _checkoutMesageTopic = _configuration["ServiceBus:CheckoutMessageTopic"];
        _paymentMessageTopic = _configuration["ServiceBus:PaymentMessageTopic"];
        _orderUpdatePaymentResultTopic = _configuration["ServiceBus:OrderUpdatePaymentResultTopic"];

        _checkoutSubscriptionName = _configuration["ServiceBus:CheckoutSubscriptionName"];

        var client = new ServiceBusClient(_connectionString);

        _checkoutProcessor = client.CreateProcessor(_checkoutMesageTopic);
        _orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _checkoutSubscriptionName);
    }

    public async Task Start()
    {
        _checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageRecieved;
        _checkoutProcessor.ProcessErrorAsync += ErrorHandler;

        await _checkoutProcessor.StartProcessingAsync();

        _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateRecieved;
        _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;

        await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _checkoutProcessor.StopProcessingAsync();
        await _checkoutProcessor.DisposeAsync();

        await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
        await _orderUpdatePaymentStatusProcessor.DisposeAsync();
    }

    private async Task OnCheckoutMessageRecieved(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        CheckoutHeaderDTO checkoutHeaderDTO = JsonConvert.DeserializeObject<CheckoutHeaderDTO>(body);

        OrderHeader orderHeader = new()
        {
            UserId = checkoutHeaderDTO.UserId,
            FirstName = checkoutHeaderDTO.FirstName,
            LastName = checkoutHeaderDTO.LastName,
            OrderDetails = new List<OrderDetail>(),
            CardNumber = checkoutHeaderDTO.CardNumber,
            CouponCode = checkoutHeaderDTO.CouponCode,
            CVV = checkoutHeaderDTO.CVV,
            DiscountTotal = checkoutHeaderDTO.DiscountTotal,
            Email = checkoutHeaderDTO.Email,
            ExpiryMonthYear = checkoutHeaderDTO.ExpiryMonthYear,
            OrderTime = DateTime.UtcNow,
            OrderTotal = checkoutHeaderDTO.OrderTotal,
            PaymentStatus = false,
            Phone = checkoutHeaderDTO.Phone,
            PickupDateTime = checkoutHeaderDTO.PickupDateTime,
        };

        foreach (var item in checkoutHeaderDTO.CartDetails)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Count = item.Count
            };

            orderHeader.TotalItems += item.Count;
            orderHeader.OrderDetails.Add(orderDetail);
        }

        await _orderRepository.AddOrder(orderHeader);

        PaymentRequestMessage paymentRequestMessage = new()
        {
            OrderId = orderHeader.Id,
            Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
            CardNumber = orderHeader.CardNumber,
            CVV = orderHeader.CVV,
            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            OrderTotal = orderHeader.OrderTotal,
            Email = orderHeader.Email
        };

        try
        {
            await _messageBus.Publish(paymentRequestMessage, _paymentMessageTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task OnOrderPaymentUpdateRecieved(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        UpdatePaymentResultMessage updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

        await _orderRepository.UpdateOrderPaymentStatus(updatePaymentResultMessage.OrderId, updatePaymentResultMessage.Status);
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }
}
