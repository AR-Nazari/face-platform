using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Contracts.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Face.Infrastructure.Services
{
    public class TestImagePipelineService : ITestImagePipelineService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestImagePipelineService> _logger;

        public TestImagePipelineService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<TestImagePipelineService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ImageNormalizeTestResponse> TestImageNormalizeAsync(ImageNormalizeTestRequest request, CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["AiServices:ImageNormalize:BaseUrl"];
            var timeoutSeconds = GetTimeout("AiServices:ImageNormalize:TimeoutSeconds", 10);

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return new ImageNormalizeTestResponse
                {
                    Success = false,
                    Message = "AiServices:ImageNormalize:BaseUrl not configured."
                };
            }

            var client = _httpClientFactory.CreateClient("ImageNormalize");
            client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await client.PostAsJsonAsync("api/v1/image-normalize", request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ImageNormalizeTestResponse>(cancellationToken: cancellationToken);
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                if (result == null)
                {
                    return new ImageNormalizeTestResponse
                    {
                        Success = false,
                        Message = "Empty response from image-normalize service.",
                        DurationMs = elapsed
                    };
                }

                // اگر سرویس پایتون خودش DurationMs را تنظیم نکرده باشد، ما مقدار گیت‌وی را می‌گذاریم
                if (result.DurationMs <= 0)
                {
                    result.DurationMs = elapsed;
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                _logger.LogError(ex, "Error while calling ImageNormalize service at {BaseUrl}", baseUrl);

                return new ImageNormalizeTestResponse
                {
                    Success = false,
                    Message = $"Error while calling image-normalize service: {ex.Message}",
                    DurationMs = elapsed
                };
            }
        }

        public async Task<FaceDetectTestResponse> TestFaceDetectPythonAsync(FaceDetectTestRequest request, CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["AiServices:FaceDetectPython:BaseUrl"];
            var timeoutSeconds = GetTimeout("AiServices:FaceDetectPython:TimeoutSeconds", 10);

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return new FaceDetectTestResponse
                {
                    Success = false,
                    Message = "AiServices:FaceDetectPython:BaseUrl not configured."
                };
            }

            var client = _httpClientFactory.CreateClient("FaceDetectPython");
            client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await client.PostAsJsonAsync("api/v1/face-detect", request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<FaceDetectTestResponse>(cancellationToken: cancellationToken);
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                if (result == null)
                {
                    return new FaceDetectTestResponse
                    {
                        Success = false,
                        Message = "Empty response from face-detect (python) service.",
                        DurationMs = elapsed
                    };
                }

                if (result.DurationMs <= 0)
                {
                    result.DurationMs = elapsed;
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                _logger.LogError(ex, "Error while calling FaceDetectPython service at {BaseUrl}", baseUrl);

                return new FaceDetectTestResponse
                {
                    Success = false,
                    Message = $"Error while calling face-detect (python) service: {ex.Message}",
                    DurationMs = elapsed
                };
            }
        }

        public Task<FaceDetectTestResponse> TestFaceDetectDelphiAsync(FaceDetectTestRequest request, CancellationToken cancellationToken = default)
        {
            // TODO: در نسخه بعدی، این متد از طریق RabbitMQ به سرویس دلفی متصل می‌شود.
            // فعلاً فقط اسکلت برمی‌گردانیم تا ساختار کلی آماده باشد.

            _logger.LogWarning("TestFaceDetectDelphiAsync is not implemented yet. It should send RPC via RabbitMQ to Delphi service.");

            var result = new FaceDetectTestResponse
            {
                Success = false,
                Message = "Delphi face-detect test not implemented yet (will use RabbitMQ RPC in the next step).",
                FacesCount = 0,
                DurationMs = 0
            };

            return Task.FromResult(result);
        }

        private int GetTimeout(string key, int defaultSeconds)
        {
            var value = _configuration[key];
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out var parsed))
            {
                return parsed;
            }

            return defaultSeconds;
        }
    }
}
