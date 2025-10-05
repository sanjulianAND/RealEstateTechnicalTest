namespace Domain.Entities;

public sealed class Property
{
    public Guid IdProperty { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = null!;
    public short? Year { get; set; }
    public Guid IdOwner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Owner? Owner { get; set; }
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();
}
