using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;

namespace Microserve.Web.Services
{

    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public  CouponService(IBaseService baesService)
        {
            _baseService = baesService;
        }
        public async Task<ResponseDto?> CreateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = couponDto,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/CreateCoupon/"

            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/Delete/"+id

            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url=StaticDetails.CouponAPIBase+ "/api/Coupon"

            }) ;
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/GetByCode/"+couponCode

            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/"+id

            });
        }

        public async Task<ResponseDto?> UpdateeCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = couponDto,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/CreateCoupon"

            });
        }
    }
}
