using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.AddImage;
using Infrastructure.Features.Properties.ChangePrice;
using Infrastructure.Features.Properties.Create;
using Infrastructure.Features.Properties.List;
using Infrastructure.Features.Properties.Update;
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

    public async Task<(IReadOnlyList<PropertyListItemDto> Items, int Total)> ListAsync(PropertyListQuery q, CancellationToken ct)
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

    public async Task<Guid> CreateAsync(PropertyCreateRequestDto dto, CancellationToken ct)
    {
        try
        {
            var owner = await _db.Owners.AsNoTracking().FirstOrDefaultAsync(o => o.IdOwner == dto.OwnerId, ct);
            if (owner is null) throw new KeyNotFoundException("Owner not found");

            var existsCode = await _db.Properties.AnyAsync(p => p.CodeInternal == dto.CodeInternal, ct);
            if (existsCode) throw new InvalidOperationException("CodeInternal already exists");

            var entity = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                CodeInternal = dto.CodeInternal,
                Year = dto.Year,
                IdOwner = dto.OwnerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Properties.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.IdProperty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, PropertyUpdateRequestDto dto, CancellationToken ct)
    {
        try
        {
            var entity = await _db.Properties.FirstOrDefaultAsync(p => p.IdProperty == id, ct);
            if (entity is null) throw new KeyNotFoundException("Property not found");

            var codeTaken = await _db.Properties.AnyAsync(p => p.CodeInternal == dto.CodeInternal && p.IdProperty != id, ct);
            if (codeTaken) throw new InvalidOperationException("CodeInternal already exists");

            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.CodeInternal = dto.CodeInternal;
            entity.Year = dto.Year;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property {Id}", id);
            throw;
        }
    }

    public async Task ChangePriceAsync(Guid id, PropertyChangePriceRequestDto dto, CancellationToken ct)
    {
        try
        {
            var entity = await _db.Properties.FirstOrDefaultAsync(p => p.IdProperty == id, ct);
            if (entity is null) throw new KeyNotFoundException("Property not found");
            if (dto.NewPrice <= 0) throw new InvalidOperationException("Price must be positive");

            entity.Price = dto.NewPrice;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing price for property {Id}", id);
            throw;
        }
    }

    public async Task<Guid> AddImageAsync(Guid id, PropertyAddImageRequestDto dto, CancellationToken ct)
    {
        try
        {
            var exists = await _db.Properties.AnyAsync(p => p.IdProperty == id, ct);
            if (!exists) throw new KeyNotFoundException("Property not found");

            var img = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                IdProperty = id,
                File = dto.File,
                Enabled = dto.Enabled,
                CreatedAt = DateTime.UtcNow
            };

            _db.PropertyImages.Add(img);
            await _db.SaveChangesAsync(ct);
            return img.IdPropertyImage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding image to property {Id}", id);
            throw;
        }
    }
}