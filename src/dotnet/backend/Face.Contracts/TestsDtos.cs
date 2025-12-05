using System;

namespace Face.Contracts.Tests
{
    public class ImageNormalizeTestRequest
    {
        /// <summary>
        /// تصویر ورودی به صورت Base64. اگر خالی باشد، از تصویر پیش‌فرض استفاده می‌شود.
        /// </summary>
        public string? ImageBase64 { get; set; }
    }

    public class ImageNormalizeTestResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public int NewWidth { get; set; }
        public int NewHeight { get; set; }

        /// <summary>
        /// تصویر خروجی نرمال‌شده به صورت Base64 (jpg).
        /// </summary>
        public string? OutputImageBase64 { get; set; }

        /// <summary>
        /// زمان پردازش به میلی‌ثانیه.
        /// </summary>
        public long DurationMs { get; set; }
    }

    public class FaceDetectTestRequest
    {
        /// <summary>
        /// تصویر ورودی به صورت Base64. اگر خالی باشد، از تصویر پیش‌فرض استفاده می‌شود.
        /// </summary>
        public string? ImageBase64 { get; set; }
    }

    public class FaceDetectTestResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// تعداد چهره‌های شناسایی‌شده.
        /// </summary>
        public int FacesCount { get; set; }

        /// <summary>
        /// زمان پردازش به میلی‌ثانیه.
        /// </summary>
        public long DurationMs { get; set; }
    }
}
