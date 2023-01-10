using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.EmailAPI.Abstractions.Repositories;
using OwlRestaurant.Services.EmailAPI.DBContexts;
using OwlRestaurant.Services.EmailAPI.Messages;
using OwlRestaurant.Services.EmailAPI.Models;

namespace OwlRestaurant.Services.EmailAPI.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly DbContextOptions<ApplicationDbContext> _context;

    public EmailRepository(DbContextOptions<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
    {
        EmailLog emailLog = new()
        {
            Email = message.Email,
            CreatedAt = DateTime.UtcNow,
            Log = $"Payment for order {message.OrderId} was {message.Status}"
        };

        await using var _db = new ApplicationDbContext(_context);
        _db.EmailLogs.Add(emailLog);
        await _db.SaveChangesAsync();
    }
}
