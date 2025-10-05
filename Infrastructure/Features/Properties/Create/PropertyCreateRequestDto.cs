namespace Infrastructure.Features.Properties.Create;

public sealed class PropertyCreateRequestDto
{
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
    public decimal Price { get; init; }
    public string CodeInternal { get; init; } = null!;
    public short? Year { get; init; }
    public Guid OwnerId { get; init; }
}
