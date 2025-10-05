namespace Infrastructure.Features.Properties.List;

public sealed class PropertyListItemDto
{
    public Guid IdProperty { get; init; }
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
    public decimal Price { get; init; }
    public short? Year { get; init; }
    public string CodeInternal { get; init; } = null!;
    public string OwnerName { get; init; } = null!;
    public string? CoverImage { get; init; }
}
