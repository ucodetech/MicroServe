using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.ResponseDtos;

namespace Microserve.Web.Services.IService
{
    public interface ILocatorService
    {
        Task<ResponseDto?> GetAllFacilitiesAsync();
        Task<ResponseDto?> GetFacilityByIdAsync(int id);
        Task<ResponseDto?> CreateLocationAsync(LocatorDTO locatorDTO);
        Task<ResponseDto?> UpdateAsync(LocatorDTO locatorDTO);
        Task<ResponseDto?> DeleteLocationAsync(int id);
        Task<ResponseDto?> SearchFacilityAsync(SearchDTO searchDTO);
        Task<ResponseDto?> ViewMapData(int locatorId);
    }
}
