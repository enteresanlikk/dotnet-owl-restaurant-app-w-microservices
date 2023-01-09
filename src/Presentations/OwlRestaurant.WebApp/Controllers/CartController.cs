using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;

    public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
    {
        _productService = productService;
        _cartService = cartService;
        _couponService = couponService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await GetCardDataAsync();
        return View(data);
    }

    public async Task<IActionResult> Checkout()
    {
        var data = await GetCardDataAsync();
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartDTO cartDTO)
    {
        try
        {
            var response = await _cartService.Checkout<ResponseDTO>(cartDTO.CartHeader);
            if (response is not null && response.Success)
            {
                return RedirectToAction(nameof(CheckoutConfirmation));
            }
        }
        catch (Exception ex)
        {

        }
        return View(cartDTO);
    }

    public async Task<IActionResult> CheckoutConfirmation()
    {
        return View();
    }

    public async Task<IActionResult> Remove(Guid id)
    {
        Guid userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? Guid.Empty.ToString());
        var response = await _cartService.DeleteFromCartAsync<ResponseDTO>(id);

        if (response is not null && response.Success)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    private async Task<CartDTO> GetCardDataAsync()
    {
        Guid userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? Guid.Empty.ToString());
        var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId);

        CartDTO data = new();
        if (response is not null && response.Success)
        {
            data = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Data));
        }

        if (data?.CartHeader is not null)
        {
            string couponCode = data.CartHeader.CouponCode;
            if (!string.IsNullOrEmpty(couponCode))
            {
                var couponReponse = await _couponService.GetCoupon<ResponseDTO>(couponCode);

                if (couponReponse is not null && couponReponse.Success)
                {
                    var coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(couponReponse.Data));

                    data.CartHeader.DiscountTotal = coupon.DiscountAmount;
                }
            }

            foreach (var item in data.CartDetails)
            {
                data.CartHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            data.CartHeader.OrderTotal -= data.CartHeader.DiscountTotal;
        }

        return data;
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
    {
        Guid userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? Guid.Empty.ToString());
        var response = await _cartService.ApplyCoupon<ResponseDTO>(cartDTO);

        if (response is not null && response.Success)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
    {
        var response = await _cartService.RemoveCoupon<ResponseDTO>(cartDTO.CartHeader.UserId);

        if (response is not null && response.Success)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }
}
