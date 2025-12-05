using MediatR;

namespace Face.Application.Users.Commands.UpdatePreferredLanguage
{
    public class UpdatePreferredLanguageCommand : IRequest
    {
        public int UserId { get; init; }
        public string PreferredLanguage { get; init; } = "fa-IR";
    }
}
