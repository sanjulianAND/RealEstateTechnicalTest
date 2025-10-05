using FluentValidation;
using Infrastructure.Features.Properties.AddImage;

namespace Application.Features.Properties.AddImage;

public sealed class PropertyAddImageValidator : AbstractValidator<PropertyAddImageRequestDto>
{
    public PropertyAddImageValidator()
    {
        RuleFor(x => x.File).NotEmpty().MaximumLength(500);
    }
}
