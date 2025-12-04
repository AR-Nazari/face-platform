using System.Threading;
using System.Threading.Tasks;
using Face.Domain.Frames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Face.Application.Frames.EventHandlers
{
    public class FramePreprocessedDomainEventHandler : INotificationHandler<FramePreprocessedDomainEvent>
    {
        private readonly ILogger<FramePreprocessedDomainEventHandler> _logger;

        public FramePreprocessedDomainEventHandler(ILogger<FramePreprocessedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(FramePreprocessedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Frame preprocessed domain event handled for FrameId: {FrameId}", notification.FrameId.Value);
            return Task.CompletedTask;
        }
    }
}
