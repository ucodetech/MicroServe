namespace Microserve.Web.Models.DTOs.ResponseDtos
{
    public class LoginResponseDTO
    {
        public AuthDTO User { get; set; }
        public string Token { get; set; }
    }
}
