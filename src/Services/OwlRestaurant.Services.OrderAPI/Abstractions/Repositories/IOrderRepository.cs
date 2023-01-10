using OwlRestaurant.Services.OrderAPI.Models;

namespace OwlRestaurant.Services.OrderAPI.Abstractions.Repositories;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader orderHeader);
    Task UpdateOrderPaymentStatus(Guid orderHeaderId, bool paid);
}
