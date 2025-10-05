using Application.Features.Properties.AddImage;
using Application.Features.Properties.ChangePrice;
using Application.Features.Properties.Create;
using Application.Features.Properties.List;
using Application.Features.Properties.Update;
using FluentValidation;
using Infrastructure.Features.Properties.AddImage;
using Infrastructure.Features.Properties.ChangePrice;
using Infrastructure.Features.Properties.Create;
using Infrastructure.Features.Properties.List;
using Infrastructure.Features.Properties.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PropertyListHandler>();
        services.AddScoped<IValidator<PropertyListQuery>, PropertyListValidator>();

        services.AddScoped<PropertyCreateHandler>();
        services.AddScoped<IValidator<PropertyCreateRequestDto>, PropertyCreateValidator>();

        services.AddScoped<PropertyUpdateHandler>();
        services.AddScoped<IValidator<PropertyUpdateRequestDto>, PropertyUpdateValidator>();

        services.AddScoped<PropertyChangePriceHandler>();
        services.AddScoped<IValidator<PropertyChangePriceRequestDto>, PropertyChangePriceValidator>();

        services.AddScoped<PropertyAddImageHandler>();
        services.AddScoped<IValidator<PropertyAddImageRequestDto>, PropertyAddImageValidator>();

        return services;
    }
}
