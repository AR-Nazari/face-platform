using System;
using System.Collections.Generic;

namespace Face.Application.Diagnostics.RabbitMq
{
    public class RabbitQueueInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public int Messages { get; set; }
        public int MessagesReady { get; set; }
        public int MessagesUnacknowledged { get; set; }
        public int Consumers { get; set; }
    }

    public class RabbitConnectionInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string PeerAddress { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public DateTime? ConnectedAtUtc { get; set; }
    }

    public class RabbitMqStatusDto
    {
        public List<RabbitQueueInfoDto> Queues { get; set; } = new();
        public List<RabbitConnectionInfoDto> Connections { get; set; } = new();
    }
}
