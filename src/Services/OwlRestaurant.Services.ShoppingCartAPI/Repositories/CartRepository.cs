using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ShoppingCartAPI.DBContexts;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;
using OwlRestaurant.Services.ShoppingCartAPI.Models;

namespace OwlRestaurant.Services.ShoppingCartAPI.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CartRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<bool> ApplyCoupon(Guid userId, string couponCode)
    {
        var cart = await _context.CartHeaders.Where(x => x.UserId == userId).FirstOrDefaultAsync();

        if (cart is null)
        {
            return false;
        }

        cart.CouponCode = couponCode;

        _context.CartHeaders.Update(cart);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ClearCartByUserIdAsync(Guid userId)
    {
        var cartHeaderItem = _context.CartHeaders.Where(c => c.UserId == userId).FirstOrDefault();
        if (cartHeaderItem is null)
        {
            var cartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cartHeaderItem.Id);
            _context.CartDetails.RemoveRange(cartDetails);
            _context.CartHeaders.Remove(cartHeaderItem);
            
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CartDTO> CreateUpdateCartAsync(CartDTO cartDTO)
    {
        Cart cart = _mapper.Map<Cart>(cartDTO);

        var hasProduct = _context.Products.AsNoTracking().Where(p => p.Id == cart.CartDetails.FirstOrDefault().ProductId).FirstOrDefault();
        if (hasProduct is null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }

        var cartHeaderItem = _context.CartHeaders.AsNoTracking().Where(c => c.UserId == cart.CartHeader.UserId).FirstOrDefault();
        if (cartHeaderItem is null)
        {
            _context.CartHeaders.Add(cart.CartHeader);

            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartDetails.FirstOrDefault().Product = null;
            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

            await _context.SaveChangesAsync();
        }
        else
        {
            var cartDetail = _context.CartDetails.AsNoTracking().Where(c => c.ProductId == cart.CartDetails.FirstOrDefault().ProductId
            && c.CartHeaderId == cartHeaderItem.Id).FirstOrDefault();

            if (cartDetail is null)
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderItem.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());

                await _context.SaveChangesAsync();
            }
        }

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> GetCartByUserIdAsync(Guid userId)
    {
        var cartHeaderItem = _context.CartHeaders.Where(c => c.UserId == userId).FirstOrDefault();
        if (cartHeaderItem is not null)
        {
            var cartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cartHeaderItem.Id).Include(c => c.Product);
            var cart = new Cart
            {
                CartHeader = cartHeaderItem,
                CartDetails = cartDetails
            };
            return _mapper.Map<CartDTO>(cart);
        }
        return null;
    }
    
    public async Task<bool> RemoveCoupon(Guid userId)
    {
        var cart = await _context.CartHeaders.Where(x => x.UserId == userId).FirstOrDefaultAsync();

        if (cart is null)
        {
            return false;
        }

        cart.CouponCode = null;

        _context.CartHeaders.Update(cart);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFromCartAsync(Guid cartDetailId)
    {
        try
        {
            var cartDetails = _context.CartDetails.Where(c => c.Id == cartDetailId).FirstOrDefault();

            int totalCartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cartDetails.CartHeaderId).Count();

            _context.CartDetails.Remove(cartDetails);

            if (totalCartDetails == 1)
            {
                var cartHeader = _context.CartHeaders.Where(c => c.Id == cartDetails.CartHeaderId).FirstOrDefault();
                _context.CartHeaders.Remove(cartHeader);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
