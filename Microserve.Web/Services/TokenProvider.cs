using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Newtonsoft.Json.Linq;

namespace Microserve.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public void clearToken()
        {
            //clear cookie
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.CookieToken);
        }

        public string? getToken()
        {
            //get cookie value which is the token
           string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.CookieToken, out token);
            return hasToken is true ? token : null;
        }

        public void setToken(string Token)
        {
            //set cookie and append the token here
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.CookieToken, Token);
        }
    }
}
