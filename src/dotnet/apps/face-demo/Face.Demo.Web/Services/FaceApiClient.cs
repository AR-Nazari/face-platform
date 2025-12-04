using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Face.Contracts.FramePreprocess;
using Face.Contracts.Tests;
using Face.Demo.Web.Models;

namespace Face.Demo.Web.Services
{
    public class FaceApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public FaceApiClient(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void ApplyAuthHeader()
        {
            if (!string.IsNullOrWhiteSpace(_authService.JwtToken))
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authService.JwtToken}");
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authService.JwtToken}");
                }
            }
            else
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
        }

        // ==== Frame Preprocess (قبلی) ====

        public async Task<FramePreprocessResponse?> PreprocessAsync(FramePreprocessRequest request)
        {
            ApplyAuthHeader();

            var response = await _httpClient.PostAsJsonAsync("/api/v1/frame-preprocess", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<FramePreprocessResponse>();
            return result;
        }

        // ==== Diagnostics: RabbitMQ ====

        public async Task<RabbitMqStatusVm?> GetRabbitMqStatusAsync()
        {
            ApplyAuthHeader();

            var response = await _httpClient.GetAsync("/api/v1/diagnostics/rabbitmq/status");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<RabbitMqStatusVm>();
            return result;
        }

        public async Task<IReadOnlyList<RabbitQueueInfoVm>?> GetRabbitMqQueuesAsync()
        {
            ApplyAuthHeader();

            var response = await _httpClient.GetAsync("/api/v1/diagnostics/rabbitmq/queues");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<RabbitQueueInfoVm>>();
            return result;
        }

        public async Task<IReadOnlyList<RabbitConnectionInfoVm>?> GetRabbitMqConnectionsAsync()
        {
            ApplyAuthHeader();

            var response = await _httpClient.GetAsync("/api/v1/diagnostics/rabbitmq/connections");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<RabbitConnectionInfoVm>>();
            return result;
        }

        // ==== Tests: ImageNormalize & FaceDetect ====

        public async Task<ImageNormalizeTestResponse?> TestImageNormalizeAsync(string? imageBase64 = null)
        {
            ApplyAuthHeader();

            var payload = new ImageNormalizeTestRequest
            {
                ImageBase64 = imageBase64
            };

            var response = await _httpClient.PostAsJsonAsync("/api/v1/tests/image-normalize", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ImageNormalizeTestResponse>();
            return result;
        }

        public async Task<FaceDetectTestResponse?> TestFaceDetectPythonAsync(string? imageBase64 = null)
        {
            ApplyAuthHeader();

            var payload = new FaceDetectTestRequest
            {
                ImageBase64 = imageBase64
            };

            var response = await _httpClient.PostAsJsonAsync("/api/v1/tests/face-detect/python", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<FaceDetectTestResponse>();
            return result;
        }

        public async Task<FaceDetectTestResponse?> TestFaceDetectDelphiAsync(string? imageBase64 = null)
        {
            ApplyAuthHeader();

            var payload = new FaceDetectTestRequest
            {
                ImageBase64 = imageBase64
            };

            var response = await _httpClient.PostAsJsonAsync("/api/v1/tests/face-detect/delphi", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<FaceDetectTestResponse>();
            return result;
        }
    }
}
