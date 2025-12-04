using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Diagnostics.RabbitMq;

namespace Face.Application.Common.Interfaces
{
    public interface IRabbitMqMonitoringService
    {
        Task<RabbitMqStatusDto> GetStatusAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RabbitQueueInfoDto>> GetQueuesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RabbitConnectionInfoDto>> GetConnectionsAsync(CancellationToken cancellationToken = default);
    }
}
