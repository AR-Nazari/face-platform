using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Frames.Commands.PreprocessFrame;
using Face.Contracts.FramePreprocess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Face.Api.Controllers
{
    [ApiController]
    [Route("api/v1/frame-preprocess")]
    public class FramePreprocessController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FramePreprocessController> _logger;

        public FramePreprocessController(
            IMediator mediator,
            ILogger<FramePreprocessController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FramePreprocessResponse), 200)]
        public async Task<ActionResult<FramePreprocessResponse>> Post(
            [FromBody] FramePreprocessRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received FramePreprocess request {FrameId}", request.FrameId);

            var stopwatch = Stopwatch.StartNew();

            var command = new PreprocessFrameCommand(request);

            var response = await _mediator.Send(command, cancellationToken);

            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("FramePreprocess for {FrameId} completed in {ElapsedMs} ms", request.FrameId, elapsedMs);

            Response.Headers["X-Elapsed-Milliseconds"] = elapsedMs.ToString();

            return Ok(response);
        }
    }
}
