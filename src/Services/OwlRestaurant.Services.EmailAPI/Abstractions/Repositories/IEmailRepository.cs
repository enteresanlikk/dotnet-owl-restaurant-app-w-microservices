using OwlRestaurant.Services.EmailAPI.Messages;
using OwlRestaurant.Services.EmailAPI.Models;

namespace OwlRestaurant.Services.EmailAPI.Abstractions.Repositories;

public interface IEmailRepository
{
    Task SendAndLogEmail(UpdatePaymentResultMessage message);
}
