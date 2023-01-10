using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OwlRestaurant.Services.CouponAPI.Abstractions.Repositories;
using OwlRestaurant.Services.CouponAPI.DTOs;

namespace OwlRestaurant.Services.CouponAPI.Controllers
{
    [Route("api/coupons")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponsController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        [Route("{couponCode}")]
        public async Task<IActionResult> GetCouponByCode(string couponCode)
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _couponRepository.GetCouponByCode(couponCode);

                response.Success = data is not null;
                response.Data = data;

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
