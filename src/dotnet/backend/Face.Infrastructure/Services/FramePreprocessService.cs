using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Contracts.FramePreprocess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Face.Infrastructure.Services
{
    public class FramePreprocessService : IFramePreprocessService
    {
        private readonly HttpClient _httpClient;
        private readonly FramePreprocessServiceOptions _options;
        private readonly ILogger<FramePreprocessService> _logger;

        public FramePreprocessService(
            HttpClient httpClient,
            IOptions<FramePreprocessServiceOptions> options,
            ILogger<FramePreprocessService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
        }

        public async Task<FramePreprocessResponse> PreprocessAsync(
            FramePreprocessRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(
                    "/api/v1/frame-preprocess",
                    request,
                    cancellationToken);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "FramePreprocess upstream responded with status code {StatusCode}",
                        httpResponse.StatusCode);

                    return new FramePreprocessResponse
                    {
                        FrameId = request.FrameId,
                        Status = "error",
                        Error = new FramePreprocessError
                        {
                            Code = "UPSTREAM_ERROR",
                            Message = $"Upstream returned {httpResponse.StatusCode}"
                        }
                    };
                }

                var result =
                    await httpResponse.Content.ReadFromJsonAsync<FramePreprocessResponse>(cancellationToken: cancellationToken);

                if (result == null)
                {
                    return new FramePreprocessResponse
                    {
                        FrameId = request.FrameId,
                        Status = "error",
                        Error = new FramePreprocessError
                        {
                            Code = "EMPTY_RESPONSE",
                            Message = "Upstream response could not be deserialized"
                        }
                    };
                }

                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout while calling FramePreprocess service.");

                return new FramePreprocessResponse
                {
                    FrameId = request.FrameId,
                    Status = "error",
                    Error = new FramePreprocessError
                    {
                        Code = "TIMEOUT",
                        Message = "Timeout while calling FramePreprocess service."
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while calling FramePreprocess service.");

                return new FramePreprocessResponse
                {
                    FrameId = request.FrameId,
                    Status = "error",
                    Error = new FramePreprocessError
                    {
                        Code = "EXCEPTION",
                        Message = ex.Message
                    }
                };
            }
        }
    }
}
