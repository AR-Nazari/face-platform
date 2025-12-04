using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Application.Diagnostics.RabbitMq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Face.Api.Controllers
{
    [ApiController]
    [Route("api/v1/diagnostics")]
    [Authorize(Roles = "Admin")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IRabbitMqMonitoringService _rabbitMqMonitoringService;
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(
            IRabbitMqMonitoringService rabbitMqMonitoringService,
            ILogger<DiagnosticsController> logger)
        {
            _rabbitMqMonitoringService = rabbitMqMonitoringService;
            _logger = logger;
        }

        [HttpGet("rabbitmq/status")]
        [ProducesResponseType(typeof(RabbitMqStatusDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RabbitMqStatusDto>> GetRabbitMqStatus(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Diagnostics: requesting RabbitMQ status.");

            var status = await _rabbitMqMonitoringService.GetStatusAsync(cancellationToken);

            return Ok(status);
        }

        [HttpGet("rabbitmq/queues")]
        [ProducesResponseType(typeof(IReadOnlyList<RabbitQueueInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<RabbitQueueInfoDto>>> GetRabbitMqQueues(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Diagnostics: requesting RabbitMQ queues.");

            var queues = await _rabbitMqMonitoringService.GetQueuesAsync(cancellationToken);

            return Ok(queues);
        }

        [HttpGet("rabbitmq/connections")]
        [ProducesResponseType(typeof(IReadOnlyList<RabbitConnectionInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<RabbitConnectionInfoDto>>> GetRabbitMqConnections(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Diagnostics: requesting RabbitMQ connections.");

            var connections = await _rabbitMqMonitoringService.GetConnectionsAsync(cancellationToken);

            return Ok(connections);
        }
    }
}
