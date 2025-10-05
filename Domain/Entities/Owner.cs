using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities;

public sealed class Owner
{
    public Guid IdOwner { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? Photo { get; set; }
    public DateTime? Birthday { get; set; }

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
