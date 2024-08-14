using Microserve.Services.AuthAPI.Models.DTOs;
using Microserve.Services.AuthAPI.Models.DTOs.RequestDTO;
using Microserve.Services.AuthAPI.Models.DTOs.ResponseDTO;
using Microsoft.AspNetCore.Identity.Data;

namespace Microserve.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> AssignRole(string email, string roleName);
        Task<string> GetLoggedInUserId();
        Task<string> GetLoggedInUserName();
        Task<IEnumerable<string>> GetLoggedInRoles();
    }
}
