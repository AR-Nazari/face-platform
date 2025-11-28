using System;
using System.Collections.Generic;

namespace Face.Contracts.FramePreprocess
{
    // Request DTO
    public class FramePreprocessRequest
    {
        public string FrameId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// camera | file | stream
        /// </summary>
        public string Source { get; set; } = "camera";

        /// <summary>
        /// UTC timestamp in ISO 8601 format
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Base64 encoded image (PNG/JPEG)
        /// </summary>
        public string ImageBase64 { get; set; } = string.Empty;

        public FramePreprocessOptions Options { get; set; } = new FramePreprocessOptions();

        public FramePreprocessMeta Meta { get; set; } = new FramePreprocessMeta();
    }

    public class FramePreprocessOptions
    {
        public int TargetWidth { get; set; } = 640;
        public int TargetHeight { get; set; } = 640;
        public bool Normalize { get; set; } = true;
        public bool ReturnDebug { get; set; } = false;
    }

    public class FramePreprocessMeta
    {
        public string CameraId { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Any extra metadata (JSON-serialized key-values)
        /// </summary>
        public Dictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();
    }

    // Response DTO
    public class FramePreprocessResponse
    {
        public string FrameId { get; set; } = string.Empty;

        /// <summary>
        /// ok | error
        /// </summary>
        public string Status { get; set; } = "ok";

        public FramePreprocessError? Error { get; set; }

        public string? PreprocessedImageBase64 { get; set; }

        public List<DetectedFace> Faces { get; set; } = new List<DetectedFace>();

        public FramePreprocessDebugInfo? Debug { get; set; }
    }

    public class FramePreprocessError
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DetectedFace
    {
        public int Id { get; set; }
        public List<int> Bbox { get; set; } = new List<int>(); // [x, y, w, h]
        public double Score { get; set; }
    }

    public class FramePreprocessDebugInfo
    {
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public int ProcessedWidth { get; set; }
        public int ProcessedHeight { get; set; }

        public List<string> PipelineSteps { get; set; } = new List<string>();
    }
}
