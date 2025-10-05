namespace Infrastructure.Features.Properties.AddImage;

public sealed class PropertyAddImageRequestDto
{
    public string File { get; init; } = null!;
    public bool Enabled { get; init; } = true;
}
