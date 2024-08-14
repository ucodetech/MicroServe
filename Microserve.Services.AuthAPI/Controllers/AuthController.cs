using Microserve.Services.AuthAPI.Helpers;
using Microserve.Services.AuthAPI.Models.DTOs.RequestDTO;
using Microserve.Services.AuthAPI.Models.DTOs.ResponseDTO;
using Microserve.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;


namespace Microserve.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private BaseResponse _response;
        private RoleDTO _roleDTO;
        private UserHelper _userHelper;
        public AuthController(IAuthService authService, UserHelper userHelper)
        {
            _authService = authService;
            _response = new();
            _userHelper = userHelper;
            _roleDTO = new();
           


        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
           
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);

            }
            _response.Message = "User created!";
            return Ok(_response);
             
        }

        [HttpPost("Login")]
        public async Task<ApiResponseDTO<LoginResponseDTO>> Login([FromBody] LoginRequestDTO model)
        {
            ApiResponseDTO<LoginResponseDTO> apiResponseDTO = new ApiResponseDTO<LoginResponseDTO>();
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User is null)
            {
                apiResponseDTO.IsSuccess = false;
                apiResponseDTO.Message = "Username or Password is Invalid!";

            }
            else
            {
                apiResponseDTO.IsSuccess = true;
                apiResponseDTO.Message = "You have logged in successfullly!";
                apiResponseDTO.Result = loginResponse;
            }
            return apiResponseDTO;
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> CreateRole([FromBody] RegistrationRequestDTO model)
        {
           
            var assignedRole = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignedRole)
            {
                _response.IsSuccess = false;
                _response.Message = "Error Encounted";
                return BadRequest(_response);

            }
            return Ok(_response);

        }

        

        
    }
}
