using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OwlRestaurant.Services.ProductAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ProductAPI.DTOs;

namespace OwlRestaurant.Services.ProductAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = new ResponseDTO();

        try
        {
            var products = await _productRepository.GetProducts();

            response.Success = true;
            response.Data = products;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return NotFound(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = new ResponseDTO();

        try
        {
            var product = await _productRepository.GetProductById(id);

            response.Success = true;
            response.Data = product;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return NotFound(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Add([FromBody] ProductDTO productDTO)
    {
        var response = new ResponseDTO();

        try
        {
            var product = await _productRepository.CreateUpdateProduct(productDTO);

            response.Success = true;
            response.Data = product;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return NotFound(response);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Update([FromBody] ProductDTO productDTO)
    {
        var response = new ResponseDTO();

        try
        {
            var product = await _productRepository.CreateUpdateProduct(productDTO);

            response.Success = true;
            response.Data = product;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return NotFound(response);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = new ResponseDTO();

        try
        {
            var status = await _productRepository.DeleteProduct(id);
            
            response.Success = status;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return NotFound(response);
    }
}
