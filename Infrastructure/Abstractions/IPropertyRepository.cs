using Infrastructure.Features.Properties.AddImage;
using Infrastructure.Features.Properties.ChangePrice;
using Infrastructure.Features.Properties.Create;
using Infrastructure.Features.Properties.List;
using Infrastructure.Features.Properties.Update;

namespace Infrastructure.Abstractions;

public interface IPropertyRepository
{
    Task<(IReadOnlyList<PropertyListItemDto> Items, int Total)> ListAsync(PropertyListQuery query, CancellationToken ct);
    Task<Guid> CreateAsync(PropertyCreateRequestDto dto, CancellationToken ct);
    Task UpdateAsync(Guid id, PropertyUpdateRequestDto dto, CancellationToken ct);
    Task ChangePriceAsync(Guid id, PropertyChangePriceRequestDto dto, CancellationToken ct);
    Task<Guid> AddImageAsync(Guid id, PropertyAddImageRequestDto dto, CancellationToken ct);
}
