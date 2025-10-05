namespace Infrastructure.Features.Properties.List;

public sealed class PropertyListRequestDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? CodeInternal { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public short? MinYear { get; init; }
    public short? MaxYear { get; init; }
    public Guid? OwnerId { get; init; }
    public string? Sort { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

public static class PropertyListRequestMapping
{
    public static PropertyListQuery ToQuery(this PropertyListRequestDto r) => new PropertyListQuery
    {
        Name = r.Name,
        Address = r.Address,
        CodeInternal = r.CodeInternal,
        MinPrice = r.MinPrice,
        MaxPrice = r.MaxPrice,
        MinYear = r.MinYear,
        MaxYear = r.MaxYear,
        OwnerId = r.OwnerId,
        Sort = r.Sort,
        Page = r.Page,
        PageSize = r.PageSize
    };
}
