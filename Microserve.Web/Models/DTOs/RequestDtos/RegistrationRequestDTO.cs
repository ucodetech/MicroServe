using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Microserve.Web.Models.DTOs.RequestDtos
{
    public class RegistrationRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
