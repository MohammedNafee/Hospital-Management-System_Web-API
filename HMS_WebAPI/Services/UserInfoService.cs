using System.Security.Claims;

namespace HMS_WebAPI.Services
{
    public class UserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (int UserId, string Role) GetUserInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID.");

            return (userId, loggedInUserRole);
        }
    }
}
