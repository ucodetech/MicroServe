namespace Microserve.Web.Services.IService
{
    public interface ITokenProvider
    {
        void setToken(string Token);
        string? getToken();
        void clearToken(); 
    }
}
