namespace Infrastructure.Features.Properties.Update;

public sealed class PropertyUpdateRequestDto
{
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string CodeInternal { get; init; } = null!;
    public short? Year { get; init; }
}
