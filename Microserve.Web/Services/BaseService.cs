using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Microserve.Web.Utility.StaticDetails;

namespace Microserve.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDTO requestDTO)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MicroServiceAPI"); // create client and give it a name 
                HttpRequestMessage message = new(); // when making a call we need Httprequestmessage and configure options on that message
                message.Headers.Add("Accept", "application/json");
                //token for authentication
                message.RequestUri = new Uri(requestDTO.Url); // specify the uri to invoke to access any api
                                                              // if Post or put we need to serialize the data receviced and add to message.content
                if (requestDTO.Data != null)
                {
                    //data is not null
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null; // this holds response
                                                         //set message method base on what request dto returns
                switch (requestDTO.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }
                apiResponse = await client.SendAsync(message); //send the response back
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "NotFound" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;

                }
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    Message = e.Message.ToString(),
                    IsSuccess = false,
                };

                return dto;
            }
            
        }
    }
}
