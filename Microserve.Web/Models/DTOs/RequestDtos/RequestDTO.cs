using static Microserve.Web.Utility.StaticDetails;

namespace Microserve.Web.Models.DTOs.RequestDtos
{
    public class RequestDTO
    {
        public ApiType ApiType { get; set; } = ApiType.GET; // defines if its get,post, put or delete
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; } //for authentication and authorization
    }
}
