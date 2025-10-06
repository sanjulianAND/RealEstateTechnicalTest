using Application.Features.Properties.ChangePrice;
using FluentAssertions;
using Infrastructure.Features.Properties.ChangePrice;
using NUnit.Framework;

namespace Tests.Validators;

public class PropertyChangePriceValidatorTests
{
    [Test]
    public void Should_fail_when_non_positive()
    {
        var v = new PropertyChangePriceValidator();
        v.Validate(new PropertyChangePriceRequestDto { NewPrice = 0 }).IsValid.Should().BeFalse();
    }

    [Test]
    public void Should_pass_when_positive()
    {
        var v = new PropertyChangePriceValidator();
        v.Validate(new PropertyChangePriceRequestDto { NewPrice = 10 }).IsValid.Should().BeTrue();
    }
}
