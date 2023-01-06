using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public CartController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await GetCardDataAsync();
        return View(data);
    }
    
    public async Task<IActionResult> Remove(Guid id)
    {
        Guid userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? Guid.Empty.ToString());
        var response = await _cartService.DeleteFromCartAsync<ResponseDTO>(id);

        CartDTO data = new();
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
            foreach (var item in data.CartDetails)
            {
                data.CartHeader.OrderTotal += (item.Product.Price * item.Count);
            }
        }

        return data;
    }
}
