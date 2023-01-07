using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;
using System.Data;

namespace OwlRestaurant.Services.ShoppingCartAPI.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _cartRepository.GetCartByUserIdAsync(userId);
                
                response.Success = true;
                response.Data = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartDTO cartDTO)
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _cartRepository.CreateUpdateCartAsync(cartDTO);

                response.Success = true;
                response.Data = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return NotFound(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartDTO cartDTO)
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _cartRepository.CreateUpdateCartAsync(cartDTO);

                response.Success = true;
                response.Data = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return NotFound(response);
        }

        [HttpDelete]
        [Route("{cartId:guid}")]
        public async Task<IActionResult> Delete(Guid cartId)
        {
            var response = new ResponseDTO();

            try
            {
                var status = await _cartRepository.RemoveFromCartAsync(cartId);

                response.Success = status;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return NotFound(response);
        }

        [HttpPost]
        [Route("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            var response = new ResponseDTO();

            try
            {
                var status = await _cartRepository.ApplyCoupon(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);

                response.Success = status;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return NotFound(response);
        }

        [HttpDelete]
        [Route("remove-coupon")]
        public async Task<IActionResult> RemoveCoupon([FromBody] Guid userId)
        {
            var response = new ResponseDTO();

            try
            {
                var status = await _cartRepository.RemoveCoupon(userId);

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
}
