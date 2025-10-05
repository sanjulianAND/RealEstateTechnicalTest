using FluentValidation;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.Update;
using Microsoft.Extensions.Logging;

namespace Application.Features.Properties.Update;

public sealed class PropertyUpdateHandler
{
    private readonly IPropertyRepository _repo;
    private readonly IValidator<PropertyUpdateRequestDto> _validator;
    private readonly ILogger<PropertyUpdateHandler> _logger;

    public PropertyUpdateHandler(IPropertyRepository repo, IValidator<PropertyUpdateRequestDto> validator, ILogger<PropertyUpdateHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task HandleAsync(Guid id, PropertyUpdateRequestDto dto, CancellationToken ct)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            await _repo.UpdateAsync(id, dto, ct);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Invalid property update request");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in PropertyUpdateHandler");
            throw;
        }
    }
}
