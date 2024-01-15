using AutoMapper;
using Microserve.Services.CouponAPI.Models;
using Microserve.Services.CouponAPI.Models.DTOs;

namespace Microserve.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            // initial mapper configuration
            var mappingConfig = new MapperConfiguration(config =>
            {
                //do the mapping here
                config.CreateMap<CouponDTO, Coupon>();
                config.CreateMap<Coupon, CouponDTO>();

            });
            return mappingConfig;
        }
    }
}
