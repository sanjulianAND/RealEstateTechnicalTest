namespace Domain.Entities;

public sealed class PropertyTrace
{
    public Guid IdPropertyTrace { get; set; }
    public Guid IdProperty { get; set; }
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = null!;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }

    public Property? Property { get; set; }
}
