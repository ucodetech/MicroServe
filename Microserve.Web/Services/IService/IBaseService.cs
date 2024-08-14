using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;

namespace Microserve.Web.Services.IService
{
    public interface IBaseService
    {
       Task<ResponseDto?> SendAsync(RequestDTO requestDTO, bool withBearer = true);
    }
}
