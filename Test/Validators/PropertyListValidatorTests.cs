using Application.Features.Properties.List;
using FluentAssertions;
using Infrastructure.Features.Properties.List;
using NUnit.Framework;

namespace Tests.Validators;

public class PropertyListValidatorTests
{
    [Test]
    public void Should_fail_when_page_is_invalid()
    {
        var v = new PropertyListValidator();
        v.Validate(new PropertyListQuery { Page = 0, PageSize = 10 }).IsValid.Should().BeFalse();
    }

    [Test]
    public void Should_pass_when_paging_is_valid()
    {
        var v = new PropertyListValidator();
        v.Validate(new PropertyListQuery { Page = 1, PageSize = 20 }).IsValid.Should().BeTrue();
    }
}
