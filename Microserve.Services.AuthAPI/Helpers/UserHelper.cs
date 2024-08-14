using System.Security.Claims;

namespace Microserve.Services.AuthAPI.Helpers
{
    public class UserHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserHelper(IHttpContextAccessor contextAccessor)
        {

            _contextAccessor = contextAccessor;

        }

        public string GetLoggedInUserId()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userId.Value ?? string.Empty;
        }

        public string GetLoggedInUserName()
        {
            var name = _contextAccessor.HttpContext?.User?.Identity?.Name;
            return name ?? string.Empty;
        }

        public IEnumerable<string> GetLoggedInUserRoles()
        {
            var roles = _contextAccessor.HttpContext?.User?.Claims?
                .Where(c=>c.Type == ClaimTypes.Role)
                .Select(c=>c.Value) ?? Enumerable.Empty<string>();
            return roles;
        }
    }
}
