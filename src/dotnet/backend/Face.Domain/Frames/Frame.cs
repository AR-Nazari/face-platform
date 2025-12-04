using System;
using Face.Domain.Common;

namespace Face.Domain.Frames
{
    public class Frame : BaseEntity
    {
        public FrameId FrameId { get; private set; }
        public DateTime TimestampUtc { get; private set; }
        public string Source { get; private set; } = "camera";

        private Frame() { }

        public Frame(FrameId frameId, DateTime timestampUtc, string source)
        {
            FrameId = frameId;
            TimestampUtc = timestampUtc;
            Source = source;
        }

        public void MarkPreprocessed()
        {
            AddDomainEvent(new FramePreprocessedDomainEvent(FrameId));
        }
    }
}
