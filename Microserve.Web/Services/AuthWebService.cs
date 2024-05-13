using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using static Microserve.Web.Utility.StaticDetails;

namespace Microserve.Web.Services
{
    public class AuthWebService : IAuthWebService
    {
        private readonly IBaseService _baseService;
        public AuthWebService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDTO,
                Url = AuthAPIBase + "/api/auth/AssignRole"

            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType= ApiType.POST,
                Data = loginRequestDTO,
                Url = AuthAPIBase + "/api/auth/Login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDTO,
                Url = AuthAPIBase+"/api/auth/Register"
            });
        }

       
    }
}
