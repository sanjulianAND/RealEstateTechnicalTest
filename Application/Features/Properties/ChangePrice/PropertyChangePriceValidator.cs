using FluentValidation;
using Infrastructure.Features.Properties.ChangePrice;

namespace Application.Features.Properties.ChangePrice;

public sealed class PropertyChangePriceValidator : AbstractValidator<PropertyChangePriceRequestDto>
{
    public PropertyChangePriceValidator()
    {
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}
