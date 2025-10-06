using Application.Features.Properties.Create;
using FluentAssertions;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.Create;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Tests.Handlers;

public class PropertyCreateHandlerTests
{
    [Test]
    public async Task Should_create_property_and_return_id()
    {
        var repo = new Mock<IPropertyRepository>();
        var logger = Mock.Of<ILogger<PropertyCreateHandler>>();
        var validator = new PropertyCreateValidator();

        var id = Guid.NewGuid();
        repo.Setup(r => r.CreateAsync(It.IsAny<PropertyCreateRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(id);

        var handler = new PropertyCreateHandler(repo.Object, validator, logger);

        var dto = new PropertyCreateRequestDto
        {
            Name = "N",
            Address = "A",
            Price = 100,
            CodeInternal = "C-1",
            OwnerId = Guid.NewGuid()
        };

        var result = await handler.HandleAsync(dto, CancellationToken.None);
        result.Should().Be(id);
        repo.Verify(r => r.CreateAsync(It.IsAny<PropertyCreateRequestDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
