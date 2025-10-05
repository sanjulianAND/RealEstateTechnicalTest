using FluentValidation;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.AddImage;
using Microsoft.Extensions.Logging;

namespace Application.Features.Properties.AddImage;

public sealed class PropertyAddImageHandler
{
    private readonly IPropertyRepository _repo;
    private readonly IValidator<PropertyAddImageRequestDto> _validator;
    private readonly ILogger<PropertyAddImageHandler> _logger;

    public PropertyAddImageHandler(IPropertyRepository repo, IValidator<PropertyAddImageRequestDto> validator, ILogger<PropertyAddImageHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> HandleAsync(Guid id, PropertyAddImageRequestDto dto, CancellationToken ct)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            return await _repo.AddImageAsync(id, dto, ct);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Invalid add image request");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in PropertyAddImageHandler");
            throw;
        }
    }
}
