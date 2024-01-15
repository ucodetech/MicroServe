using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.ResponseDtos;

namespace Microserve.Web.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> UpdateeCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
