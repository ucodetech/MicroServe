using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using static Microserve.Web.Utility.StaticDetails;

namespace Microserve.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDTO,
                Url = StaticDetails.AuthAPIBase + "/api/auth/AssignRole"

            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType= ApiType.POST,
                Data = loginRequestDTO,
                Url = StaticDetails.AuthAPIBase+"/auth/Login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDTO,
                Url = StaticDetails.AuthAPIBase+"/api/auth/Register"
            });
        }

        public async Task<ResponseDto?> IsUserExistAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Data = registrationRequestDTO,
                Url = StaticDetails.AuthAPIBase + "/api/auth/IsUserExist"
            });
        }
    }
}
