using Application.Features.Properties.ChangePrice;
using FluentAssertions;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.ChangePrice;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Tests.Handlers;

public class PropertyChangePriceHandlerTests
{
    [Test]
    public async Task Should_call_repo_when_valid()
    {
        var repo = new Mock<IPropertyRepository>();
        var logger = Mock.Of<ILogger<PropertyChangePriceHandler>>();
        var validator = new PropertyChangePriceValidator();

        var handler = new PropertyChangePriceHandler(repo.Object, validator, logger);
        var dto = new PropertyChangePriceRequestDto { NewPrice = 100 };

        await handler.HandleAsync(Guid.NewGuid(), dto, CancellationToken.None);

        repo.Verify(r => r.ChangePriceAsync(It.IsAny<Guid>(), dto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Should_throw_when_price_invalid()
    {
        var repo = new Mock<IPropertyRepository>();
        var logger = Mock.Of<ILogger<PropertyChangePriceHandler>>();
        var validator = new PropertyChangePriceValidator();
        var handler = new PropertyChangePriceHandler(repo.Object, validator, logger);

        Func<Task> act = async () => await handler.HandleAsync(Guid.NewGuid(), new PropertyChangePriceRequestDto { NewPrice = 0 }, CancellationToken.None);
        act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
