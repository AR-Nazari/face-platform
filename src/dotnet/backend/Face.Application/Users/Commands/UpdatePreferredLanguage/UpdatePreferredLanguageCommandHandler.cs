using System;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Face.Application.Users.Commands.UpdatePreferredLanguage
{
    public class UpdatePreferredLanguageCommandHandler : IRequestHandler<UpdatePreferredLanguageCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdatePreferredLanguageCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePreferredLanguageCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException($"User with Id {request.UserId} was not found.");
            }

            user.PreferredLanguage = string.IsNullOrWhiteSpace(request.PreferredLanguage)
                ? "fa-IR"
                : request.PreferredLanguage;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
