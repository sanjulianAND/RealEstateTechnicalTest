using Application.Abstractions.Ports;
using FluentValidation;
using Infrastructure.Features.Properties.List;
using Microsoft.Extensions.Logging;

namespace Application.Features.Properties.List;

public sealed class PropertyListHandler
{
    private readonly IPropertyRepository _repo;
    private readonly IValidator<PropertyListQuery> _validator;
    private readonly ILogger<PropertyListHandler> _logger;

    public PropertyListHandler(
        IPropertyRepository repo,
        IValidator<PropertyListQuery> validator,
        ILogger<PropertyListHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<(IReadOnlyList<PropertyListItemDto> Items, int Total)> HandleAsync(
        PropertyListQuery query, CancellationToken ct = default)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _repo.ListAsync(query, ct);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Invalid property list query");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in PropertyListHandler");
            throw;
        }
    }
}
