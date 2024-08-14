using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;

namespace Microserve.Web.Services
{

    public class LocatorService : ILocatorService
    {
        private readonly IBaseService _baseService;
        public  LocatorService(IBaseService baesService)
        {
            _baseService = baesService;
        }
        public async Task<ResponseDto?> CreateLocationAsync(LocatorDTO locatorDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = locatorDTO,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/CreateLocator/"

            });
        }

        public async Task<ResponseDto?> DeleteLocationAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/Delete/"+id

            });
        }
        


        public async Task<ResponseDto?> GetAllFacilitiesAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url=StaticDetails.LocatorAPIBase+ "/api/locator/GetAll/"

            }) ;
        }

        public async Task<ResponseDto?> GetFacilityByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/GetFacility/" + id

            });
        }

        public async Task<ResponseDto?> UpdateAsync(LocatorDTO locatorDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = locatorDTO,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/UpdateLocator"

            });
        } 
        
        public async Task<ResponseDto?> SearchFacilityAsync(SearchDTO searchDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = searchDTO,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/GetAllSearch"

            });
        }

        public async Task<ResponseDto?> ViewMapData(int locatorId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.LocatorAPIBase + "/api/locator/ViewMap/" + locatorId

            });
        }
    }
}
