using Application.Features.Properties.Update;
using FluentAssertions;
using Infrastructure.Features.Properties.Update;
using NUnit.Framework;

namespace Tests.Validators;

public class PropertyUpdateValidatorTests
{
    [Test]
    public void Should_fail_when_fields_missing()
    {
        var v = new PropertyUpdateValidator();
        var dto = new PropertyUpdateRequestDto { Name = "", Address = "", CodeInternal = "" };
        v.Validate(dto).IsValid.Should().BeFalse();
    }

    [Test]
    public void Should_pass_when_fields_ok()
    {
        var v = new PropertyUpdateValidator();
        var dto = new PropertyUpdateRequestDto { Name = "N", Address = "A", CodeInternal = "C-1", Year = 2021 };
        v.Validate(dto).IsValid.Should().BeTrue();
    }
}
