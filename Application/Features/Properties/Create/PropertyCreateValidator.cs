using FluentValidation;
using Infrastructure.Features.Properties.Create;

namespace Application.Features.Properties.Create;

public sealed class PropertyCreateValidator : AbstractValidator<PropertyCreateRequestDto>
{
    public PropertyCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CodeInternal).NotEmpty().MaximumLength(50);
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
