
using Face.Application.Auth.Login;

namespace Face.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string userName, string role);
    }
}
