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

        public HomeController(IProductService productService)
        {
            _productService = productService;
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
    }
}