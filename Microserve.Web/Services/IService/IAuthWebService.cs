using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;

namespace Microserve.Web.Services.IService
{
    public interface IAuthWebService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO);
    }
}
