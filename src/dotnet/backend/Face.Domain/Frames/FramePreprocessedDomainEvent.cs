using Face.Domain.Common;

namespace Face.Domain.Frames
{
    public class FramePreprocessedDomainEvent : DomainEvent
    {
        public FrameId FrameId { get; }

        public FramePreprocessedDomainEvent(FrameId frameId)
        {
            FrameId = frameId;
        }
    }
}
