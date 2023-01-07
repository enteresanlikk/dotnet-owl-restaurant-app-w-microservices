using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.CouponAPI.Models;

namespace OwlRestaurant.Services.CouponAPI.DBContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = Guid.NewGuid(),
                Code = "10FF",
                DiscountAmount = 10
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                Code = "20FF",
                DiscountAmount = 20
            }
        ); ;
    }
}
