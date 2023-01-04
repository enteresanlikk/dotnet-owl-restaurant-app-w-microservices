using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDTO>(model);

                if (response is not null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(id);

            ProductDTO data = new();
            if (response is not null && response.Success)
            {
                data = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));

                return View(data);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync<ResponseDTO>(model);

                if (response is not null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(id);

            ProductDTO data = new();
            if (response is not null && response.Success)
            {
                data = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));

                return View(data);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.DeleteProductAsync<ResponseDTO>(model.Id);

                if (response is not null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
