using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Face.Application.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(IApplicationDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var normalizedUserName = request.UserName.Trim();
            var passwordHash = HashPassword(request.Password);

            var user = await _dbContext.Users
                .Where(u => u.UserName == normalizedUserName && u.PasswordHash == passwordHash)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var preferredLanguage = string.IsNullOrWhiteSpace(user.PreferredLanguage)
                ? "fa-IR"
                : user.PreferredLanguage;

            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.UserName,
                user.Role,
                preferredLanguage);

            return new LoginResultDto
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role,
                PreferredLanguage = preferredLanguage
            };
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            var builder = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
