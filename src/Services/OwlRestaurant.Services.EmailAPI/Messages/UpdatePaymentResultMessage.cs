﻿namespace OwlRestaurant.Services.EmailAPI.Messages;

public class UpdatePaymentResultMessage
{
    public Guid OrderId { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
}