using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Features.Properties.List;
using Application.Features.Properties.List;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PropertyListHandler>();
        services.AddScoped<IValidator<PropertyListQuery>, PropertyListValidator>();
        return services;
    }
}
