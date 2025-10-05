using Application.Features.Properties.List;
using Infrastructure.Features.Properties.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RealEstateTechnicalTest.Controllers;

[ApiController]
[Route("v1/properties")]
public sealed class PropertiesController : ControllerBase
{
    private readonly PropertyListHandler _handler;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(PropertyListHandler handler, ILogger<PropertiesController> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PropertyListRequestDto request, CancellationToken ct)
    {
        try
        {
            var query = request.ToQuery();
            var (items, total) = await _handler.HandleAsync(query, ct);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /v1/properties failed");
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }
}
