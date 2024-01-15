namespace Microserve.Services.AuthAPI.Models.DTOs.ResponseDTO
{
    public class LoginResponseDTO
    {
        public AuthDTO User { get; set; }
        public string Token { get; set; }
    }
}
