namespace Domain.Entities;

public sealed class PropertyImage
{
    public Guid IdPropertyImage { get; set; }
    public Guid IdProperty { get; set; }
    public string File { get; set; } = null!;
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }

    public Property? Property { get; set; }
}
