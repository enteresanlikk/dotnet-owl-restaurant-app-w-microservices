using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.OrderAPI.Abstractions.Repositories;
using OwlRestaurant.Services.OrderAPI.DBContexts;
using OwlRestaurant.Services.OrderAPI.Models;

namespace OwlRestaurant.Services.OrderAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContext;

    public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddOrder(OrderHeader orderHeader)
    {
        await using var _db = new ApplicationDbContext(_dbContext);
        _db.OrderHeaders.Add(orderHeader);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task UpdateOrderPaymentStatus(Guid orderHeaderId, bool paid)
    {
        await using var _db = new ApplicationDbContext(_dbContext);
        var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.Id == orderHeaderId);
        if (orderHeaderFromDb is not null)
        {
            orderHeaderFromDb.PaymentStatus = paid;
            await _db.SaveChangesAsync();
        }
    }
}
