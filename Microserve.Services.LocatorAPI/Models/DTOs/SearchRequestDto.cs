namespace Microserve.Services.LocatorAPI.Models.DTOs
{
    public class SearchRequestDto
    {
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
    }
}