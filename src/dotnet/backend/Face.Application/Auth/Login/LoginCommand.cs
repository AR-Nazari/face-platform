
using MediatR;

namespace Face.Application.Auth.Login
{
    public class LoginCommand : IRequest<LoginResultDto>
    {
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
