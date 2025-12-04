using Face.Application.Common.Interfaces;
using Face.Infrastructure.Identity;
using Face.Infrastructure.Persistence;
using Face.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Face.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? "Server=.;Database=FacePlatform;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // AI services (HTTP-based for now; در نسخه‌های بعدی می‌توانیم آن را RabbitMQ کنیم)
            services.AddHttpClient<IFramePreprocessService, FramePreprocessService>();

            // RabbitMQ Management (برای مانیتورینگ صف‌ها و سرویس‌ها)
            services.Configure<RabbitMqManagementOptions>(
                configuration.GetSection("RabbitMq:Management"));

            services.AddHttpClient<IRabbitMqMonitoringService, RabbitMqMonitoringService>();

            // سرویس تست زنجیره پردازش تصویر
            services.AddScoped<ITestImagePipelineService, TestImagePipelineService>();

            // Options for AI services
            services.Configure<FramePreprocessServiceOptions>(
                configuration.GetSection("AiServices:FramePreprocess"));

            // JWT
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
