using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.CouponAPI.Abstractions.Repositories;
using OwlRestaurant.Services.CouponAPI.DBContexts;
using OwlRestaurant.Services.CouponAPI.DTOs;

namespace OwlRestaurant.Services.CouponAPI.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CouponRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CouponDTO> GetCouponByCode(string couponCode)
    {
        var coupon = await _context.Coupons.Where(c => c.Code == couponCode).FirstOrDefaultAsync();

        return _mapper.Map<CouponDTO>(coupon);
    }
}
