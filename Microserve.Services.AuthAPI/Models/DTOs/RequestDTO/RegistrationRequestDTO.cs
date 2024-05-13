using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Microserve.Services.AuthAPI.Models.DTOs.RequestDTO
{
    public class RegistrationRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
