namespace Face.Infrastructure.Services
{
    public class RabbitMqManagementOptions
    {
        public string BaseUrl { get; set; } = "http://localhost:15672/api";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}
