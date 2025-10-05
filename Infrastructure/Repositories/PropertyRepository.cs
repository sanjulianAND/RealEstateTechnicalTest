using Application.Abstractions.Ports;
using Infrastructure.Features.Properties.List;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public sealed class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _db;
    private readonly ILogger<PropertyRepository> _logger;

    public PropertyRepository(RealEstateDbContext db, ILogger<PropertyRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<(IReadOnlyList<PropertyListItemDto> Items, int Total)> ListAsync(
        PropertyListQuery q, CancellationToken ct)
    {
        try
        {
            var query = _db.Properties.AsNoTracking()
                .Include(p => p.Owner)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Name)) query = query.Where(p => p.Name.Contains(q.Name));
            if (!string.IsNullOrWhiteSpace(q.Address)) query = query.Where(p => p.Address.Contains(q.Address));
            if (!string.IsNullOrWhiteSpace(q.CodeInternal)) query = query.Where(p => p.CodeInternal == q.CodeInternal);
            if (q.OwnerId.HasValue) query = query.Where(p => p.IdOwner == q.OwnerId.Value);
            if (q.MinPrice.HasValue) query = query.Where(p => p.Price >= q.MinPrice.Value);
            if (q.MaxPrice.HasValue) query = query.Where(p => p.Price <= q.MaxPrice.Value);
            if (q.MinYear.HasValue) query = query.Where(p => p.Year >= q.MinYear.Value);
            if (q.MaxYear.HasValue) query = query.Where(p => p.Year <= q.MaxYear.Value);

            query = q.Sort switch
            {
                "price" => query.OrderBy(p => p.Price),
                "-price" => query.OrderByDescending(p => p.Price),
                "year" => query.OrderBy(p => p.Year),
                "-year" => query.OrderByDescending(p => p.Year),
                "createdAt" => query.OrderBy(p => p.CreatedAt),
                "-createdAt" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var total = await query.CountAsync(ct);

            var items = await query
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .Select(p => new PropertyListItemDto
                {
                    IdProperty = p.IdProperty,
                    Name = p.Name,
                    Address = p.Address,
                    Price = p.Price,
                    Year = p.Year,
                    CodeInternal = p.CodeInternal,
                    OwnerName = p.Owner!.Name,
                    CoverImage = p.Images.Where(i => i.Enabled)
                                           .OrderBy(i => i.CreatedAt)
                                           .Select(i => i.File)
                                           .FirstOrDefault()
                })
                .ToListAsync(ct);

            return (items, total);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying properties");
            throw;
        }
    }
}
