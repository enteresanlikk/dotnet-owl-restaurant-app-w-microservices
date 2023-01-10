using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.EmailAPI.Models;

namespace OwlRestaurant.Services.EmailAPI.DBContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<EmailLog> EmailLogs { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
