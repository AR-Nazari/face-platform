using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Face.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // MediatR v11 registration: scan handlers from this assembly
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // AutoMapper profiles from this assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
