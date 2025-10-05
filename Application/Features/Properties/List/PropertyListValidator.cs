using FluentValidation;
using Infrastructure.Features.Properties.List;

namespace Application.Features.Properties.List;

public sealed class PropertyListValidator : AbstractValidator<PropertyListQuery>
{
    public PropertyListValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);
        RuleFor(x => x.MaxYear).GreaterThanOrEqualTo(x => x.MinYear)
            .When(x => x.MinYear.HasValue && x.MaxYear.HasValue);
    }
}
