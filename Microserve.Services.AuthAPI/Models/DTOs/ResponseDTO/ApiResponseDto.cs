namespace Microserve.Services.AuthAPI.Models.DTOs.ResponseDTO
{
    public class ApiResponseDTO<T> : BaseResponse
    {
        public T? Result { get; set; }
       
    }
}
