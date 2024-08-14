using AutoMapper;
using Microserve.Services.LocatorAPI.Models;
using Microserve.Services.LocatorAPI.Models.DTOs;

namespace Microserve.Services.LocatorAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            // initial mapper configuration
            var mappingConfig = new MapperConfiguration(config =>
            {
                //do the mapping here
                config.CreateMap<LocatorDTO, Locator>().ReverseMap();
                config.CreateMap<Locator, ResultDTO>().ReverseMap();

            });
            return mappingConfig;
        }
    }
}
