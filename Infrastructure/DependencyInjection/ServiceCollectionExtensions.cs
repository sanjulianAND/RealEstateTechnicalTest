using Application.Abstractions.Ports;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<RealEstateDbContext>(opt =>
            opt.UseSqlServer(cfg.GetConnectionString("RealEstate")));

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        return services;
    }
}
