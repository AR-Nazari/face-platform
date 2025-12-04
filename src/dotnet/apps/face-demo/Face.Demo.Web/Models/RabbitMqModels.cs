namespace Face.Demo.Web.Models
{
    public class RabbitQueueInfoVm
    {
        public string Name { get; set; } = string.Empty;
        public int Messages { get; set; }
        public int MessagesReady { get; set; }
        public int MessagesUnacknowledged { get; set; }
        public int Consumers { get; set; }
    }

    public class RabbitConnectionInfoVm
    {
        public string Name { get; set; } = string.Empty;
        public string PeerAddress { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string? ConnectedAtUtc { get; set; }
    }

    public class RabbitMqStatusVm
    {
        public List<RabbitQueueInfoVm> Queues { get; set; } = new();
        public List<RabbitConnectionInfoVm> Connections { get; set; } = new();
    }
}
