using HelpDeskTracker.Infrastructure.Auth;

namespace HelpDeskTracker.WebApi.Application.Auth
{
    public class JwtAuthenticationDataProvider
    {
        private readonly JwtManager _jwtManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthenticationDataProvider(JwtManager jwtManager, IHttpContextAccessor httpContextAccessor)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
