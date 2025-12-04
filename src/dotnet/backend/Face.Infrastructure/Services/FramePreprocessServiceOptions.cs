namespace Face.Infrastructure.Services
{
    public class FramePreprocessServiceOptions
    {
        public string BaseUrl { get; set; } = "http://localhost:8010";
        public int TimeoutSeconds { get; set; } = 5;
    }
}
