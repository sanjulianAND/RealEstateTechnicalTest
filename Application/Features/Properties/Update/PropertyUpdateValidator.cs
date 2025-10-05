using FluentValidation;
using Infrastructure.Features.Properties.Update;

namespace Application.Features.Properties.Update;

public sealed class PropertyUpdateValidator : AbstractValidator<PropertyUpdateRequestDto>
{
    public PropertyUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CodeInternal).NotEmpty().MaximumLength(50);
    }
}
