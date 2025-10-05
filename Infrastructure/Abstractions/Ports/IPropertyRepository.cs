using Infrastructure.Features.Properties.List;

namespace Application.Abstractions.Ports;

public interface IPropertyRepository
{
    Task<(IReadOnlyList<PropertyListItemDto> Items, int Total)> ListAsync(PropertyListQuery query, CancellationToken ct);
}
