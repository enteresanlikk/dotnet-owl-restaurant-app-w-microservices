using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;
using OwlRestaurant.Services.ShoppingCartAPI.Messages;
using System.Data;

namespace OwlRestaurant.Services.ShoppingCartAPI.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMessageBus _messageBus;

        public CartsController(ICartRepository cartRepository, ICouponRepository couponRepository, IMessageBus messageBus)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _messageBus = messageBus;
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
        public async Task<IActionResult> CreateUpdate([FromBody] CartDTO cartDTO)
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

        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(CheckoutHeaderDTO checkoutHeaderDTO)
        {
            var response = new ResponseDTO();

            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserIdAsync(checkoutHeaderDTO.UserId);
                if (cartDTO is null)
                {
                    return BadRequest();
                }
                checkoutHeaderDTO.CartDetails = cartDTO.CartDetails;

                string couponCode = cartDTO.CartHeader.CouponCode;
                if (!string.IsNullOrEmpty(couponCode))
                {
                    CouponDTO coupon = await _couponRepository.GetCoupon(couponCode);

                    if (checkoutHeaderDTO.DiscountTotal != coupon.DiscountAmount)
                    {
                        response.Success = false;
                        response.ErrorMessages = new List<string> { "Coupon is not valid" };
                        response.Message = "Coupon is not valid";
                        return Ok(response);
                    }
                }

                await _messageBus.Publish(checkoutHeaderDTO, "checkoutmessagetopic");

                await _cartRepository.ClearCartByUserIdAsync(checkoutHeaderDTO.UserId);

                response.Success = true;

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
