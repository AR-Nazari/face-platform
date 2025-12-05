using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Users.Commands.UpdatePreferredLanguage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Face.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public class UpdateLanguageRequestDto
        {
            public string PreferredLanguage { get; set; } = "fa-IR";
        }

        /// <summary>
        /// Update preferred UI language for the current authenticated user.
        /// </summary>
        [HttpPatch("me/language")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateMyLanguage(
            [FromBody] UpdateLanguageRequestDto request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User id is not available in the access token." });
            }

            var cmd = new UpdatePreferredLanguageCommand
            {
                UserId = userId,
                PreferredLanguage = request.PreferredLanguage
            };

            await _mediator.Send(cmd, cancellationToken);

            _logger.LogInformation("User {UserId} updated preferred language to {Lang}", userId, request.PreferredLanguage);

            return NoContent();
        }
    }
}
