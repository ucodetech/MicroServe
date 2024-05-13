using System.ComponentModel.DataAnnotations;

namespace Microserve.Services.AuthAPI.Models.DTOs.RequestDTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
