using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;
using System.Diagnostics;

namespace OwlRestaurant.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProductsAsync<ResponseDTO>();

            List<ProductDTO> data = new();
            if (response is not null && response.Success)
            {
                data = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Data));
            }

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Detail(Guid id)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(id);

            if (response is not null && response.Success)
            {
                var data = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));

                return View(data);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Detail(ProductDTO productDTO)
        {
            Guid userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? Guid.Empty.ToString());
            CartDTO cartDTO = new()
            {
                CartHeader = new()
                {
                    UserId = userId
                }
            };

            CartDetailDTO cartDetailDTO = new()
            {
                Count = productDTO.Count,
                ProductId = productDTO.Id
            };

            var response = await _productService.GetProductByIdAsync<ResponseDTO>(productDTO.Id);

            if (response is not null && response.Success)
            {
                var data = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));

                cartDetailDTO.Product = data;
            }

            cartDTO.CartDetails = new List<CartDetailDTO>()
            {
                cartDetailDTO
            };

            var addToCartResponse = await _cartService.CreateCartAsync<ResponseDTO>(cartDTO);
            if (addToCartResponse is not null && addToCartResponse.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDTO);
        }
    }
}