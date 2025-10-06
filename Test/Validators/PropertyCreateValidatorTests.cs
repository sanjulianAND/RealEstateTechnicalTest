using Application.Features.Properties.Create;
using FluentAssertions;
using Infrastructure.Features.Properties.Create;
using NUnit.Framework;

namespace Tests.Validators;

public class PropertyCreateValidatorTests
{
    [Test]
    public void Should_fail_when_required_fields_missing()
    {
        var v = new PropertyCreateValidator();
        var dto = new PropertyCreateRequestDto { Price = 0, OwnerId = Guid.Empty, Name = "", Address = "", CodeInternal = "" };
        var result = v.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Test]
    public void Should_pass_with_valid_payload()
    {
        var v = new PropertyCreateValidator();
        var dto = new PropertyCreateRequestDto
        {
            Name = "A",
            Address = "B",
            Price = 100,
            CodeInternal = "X-1",
            Year = 2020,
            OwnerId = Guid.NewGuid()
        };
        v.Validate(dto).IsValid.Should().BeTrue();
    }
}
