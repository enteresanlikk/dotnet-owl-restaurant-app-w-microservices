using OwlRestaurant.Integration.MessageBus;

namespace OwlRestaurant.Services.PaymentAPI.Messages;

public class UpdatePaymentResultMessage : BaseMessage
{
    public Guid OrderId { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
}
