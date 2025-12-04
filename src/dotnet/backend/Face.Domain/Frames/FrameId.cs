using System;
using System.Collections.Generic;
using Face.Domain.Common;

namespace Face.Domain.Frames
{
    public sealed class FrameId : ValueObject
    {
        public string Value { get; }

        private FrameId(string value)
        {
            Value = value;
        }

        public static FrameId Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("FrameId cannot be empty.", nameof(value));

            return new FrameId(value);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
