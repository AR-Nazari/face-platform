using System;
using Face.Domain.Common;

namespace Face.Domain.Frames
{
    public class FramePreprocessedDomainEvent : DomainEvent
    {
        public FrameId FrameId { get; init; } = default!;

        public FramePreprocessedDomainEvent(FrameId frameId)
        {
            FrameId = frameId ?? throw new ArgumentNullException(nameof(frameId));
        }
    }
}
