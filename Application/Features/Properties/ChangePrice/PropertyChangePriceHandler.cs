using FluentValidation;
using Infrastructure.Abstractions;
using Infrastructure.Features.Properties.ChangePrice;
using Microsoft.Extensions.Logging;

namespace Application.Features.Properties.ChangePrice;

public sealed class PropertyChangePriceHandler
{
    private readonly IPropertyRepository _repo;
    private readonly IValidator<PropertyChangePriceRequestDto> _validator;
    private readonly ILogger<PropertyChangePriceHandler> _logger;

    public PropertyChangePriceHandler(IPropertyRepository repo, IValidator<PropertyChangePriceRequestDto> validator, ILogger<PropertyChangePriceHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task HandleAsync(Guid id, PropertyChangePriceRequestDto dto, CancellationToken ct)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            await _repo.ChangePriceAsync(id, dto, ct);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Invalid change price request");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in PropertyChangePriceHandler");
            throw;
        }
    }
}
