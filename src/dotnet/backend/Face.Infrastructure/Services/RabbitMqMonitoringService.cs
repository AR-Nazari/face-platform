using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Application.Diagnostics.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Face.Infrastructure.Services
{
    public class RabbitMqMonitoringService : IRabbitMqMonitoringService
    {
        private readonly HttpClient _httpClient;
        private readonly RabbitMqManagementOptions _options;
        private readonly ILogger<RabbitMqMonitoringService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public RabbitMqMonitoringService(
            HttpClient httpClient,
            IOptions<RabbitMqManagementOptions> options,
            ILogger<RabbitMqMonitoringService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/') + "/");
            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.UserName}:{_options.Password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

        public async Task<RabbitMqStatusDto> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            var queues = await GetQueuesAsync(cancellationToken);
            var connections = await GetConnectionsAsync(cancellationToken);

            return new RabbitMqStatusDto
            {
                Queues = new List<RabbitQueueInfoDto>(queues),
                Connections = new List<RabbitConnectionInfoDto>(connections)
            };
        }

        public async Task<IReadOnlyList<RabbitQueueInfoDto>> GetQueuesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.GetAsync("queues", cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var doc = JsonDocument.Parse(json);

                var result = new List<RabbitQueueInfoDto>();

                foreach (var elt in doc.RootElement.EnumerateArray())
                {
                    var dto = new RabbitQueueInfoDto
                    {
                        Name = elt.GetPropertyOrDefault("name", string.Empty),
                        Messages = elt.GetPropertyOrDefault("messages", 0),
                        MessagesReady = elt.GetPropertyOrDefault("messages_ready", 0),
                        MessagesUnacknowledged = elt.GetPropertyOrDefault("messages_unacknowledged", 0),
                        Consumers = elt.GetPropertyOrDefault("consumers", 0)
                    };

                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while querying RabbitMQ queues from {BaseUrl}", _options.BaseUrl);
                return Array.Empty<RabbitQueueInfoDto>();
            }
        }

        public async Task<IReadOnlyList<RabbitConnectionInfoDto>> GetConnectionsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.GetAsync("connections", cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var doc = JsonDocument.Parse(json);

                var result = new List<RabbitConnectionInfoDto>();

                foreach (var elt in doc.RootElement.EnumerateArray())
                {
                    var dto = new RabbitConnectionInfoDto
                    {
                        Name = elt.GetPropertyOrDefault("name", string.Empty),
                        PeerAddress = elt.GetPropertyOrDefault("peer_host", string.Empty),
                        ClientName = elt.TryGetProperty("client_properties", out var cp) && cp.TryGetProperty("product", out var product)
                            ? product.GetPropertyOrDefault("value", string.Empty)
                            : string.Empty,
                        ConnectedAtUtc = elt.TryGetProperty("connected_at", out var connectedAt) && connectedAt.ValueKind == JsonValueKind.String
                            ? ParseDateTime(connectedAt.GetString())
                            : null
                    };

                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while querying RabbitMQ connections from {BaseUrl}", _options.BaseUrl);
                return Array.Empty<RabbitConnectionInfoDto>();
            }
        }

        private static DateTime? ParseDateTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, out var dt))
                return dt.ToUniversalTime();

            return null;
        }
    }

    internal static class JsonElementExtensions
    {
        public static string GetPropertyOrDefault(this JsonElement element, string name, string defaultValue)
        {
            if (element.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String)
            {
                return p.GetString() ?? defaultValue;
            }
            return defaultValue;
        }

        public static int GetPropertyOrDefault(this JsonElement element, string name, int defaultValue)
        {
            if (element.TryGetProperty(name, out var p))
            {
                if (p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out var value))
                    return value;
            }

            return defaultValue;
        }
    }
}
