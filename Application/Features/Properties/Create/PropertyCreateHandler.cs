using FluentValidation;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.Create;
using Microsoft.Extensions.Logging;

namespace Application.Features.Properties.Create;

public sealed class PropertyCreateHandler
{
    private readonly IPropertyRepository _repo;
    private readonly IValidator<PropertyCreateRequestDto> _validator;
    private readonly ILogger<PropertyCreateHandler> _logger;

    public PropertyCreateHandler(IPropertyRepository repo, IValidator<PropertyCreateRequestDto> validator, ILogger<PropertyCreateHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> HandleAsync(PropertyCreateRequestDto dto, CancellationToken ct)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            return await _repo.CreateAsync(dto, ct);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Invalid property create request");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in PropertyCreateHandler");
            throw;
        }
    }
}
