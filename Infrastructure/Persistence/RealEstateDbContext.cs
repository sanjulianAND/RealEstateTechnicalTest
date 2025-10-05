using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
        : base(options) { }
}
