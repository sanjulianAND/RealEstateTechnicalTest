using Application.Features.Properties.AddImage;
using Application.Features.Properties.ChangePrice;
using Application.Features.Properties.Create;
using Application.Features.Properties.List;
using Application.Features.Properties.Update;
using Infrastructure.Features.Properties.AddImage;
using Infrastructure.Features.Properties.ChangePrice;
using Infrastructure.Features.Properties.Create;
using Infrastructure.Features.Properties.List;
using Infrastructure.Features.Properties.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace RealEstateTechnicalTest.Controllers;

[ApiController]
[Route("v1/properties")]
public sealed class PropertiesController : ControllerBase
{
    private readonly PropertyListHandler _list;
    private readonly PropertyCreateHandler _create;
    private readonly PropertyUpdateHandler _update;
    private readonly PropertyChangePriceHandler _changePrice;
    private readonly PropertyAddImageHandler _addImage;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        PropertyListHandler list,
        PropertyCreateHandler create,
        PropertyUpdateHandler update,
        PropertyChangePriceHandler changePrice,
        PropertyAddImageHandler addImage,
        ILogger<PropertiesController> logger)
    {
        _list = list;
        _create = create;
        _update = update;
        _changePrice = changePrice;
        _addImage = addImage;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PropertyListRequestDto request, CancellationToken ct)
    {
        try
        {
            var (items, total) = await _list.HandleAsync(request.ToQuery(), ct);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /v1/properties failed");
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PropertyCreateRequestDto request, CancellationToken ct)
    {
        try
        {
            var id = await _create.HandleAsync(request, ct);
            return Created($"/v1/properties/{id}", new { ok = true, id, message = "Property created" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { ok = false, error = "Owner not found" });
        }
        catch (InvalidOperationException ioe) when (ioe.Message.Contains("CodeInternal"))
        {
            return Conflict(new { ok = false, error = "CodeInternal already exists" });
        }
        catch (DbUpdateException dbx) when (dbx.InnerException is SqlException)
        {
            return Conflict(new { ok = false, error = "Database constraint violation" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST /v1/properties failed");
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PropertyUpdateRequestDto request, CancellationToken ct)
    {
        try
        {
            await _update.HandleAsync(id, request, ct);
            return Ok(new { ok = true, id, message = "Property updated" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { ok = false, error = "Property not found" });
        }
        catch (InvalidOperationException ioe) when (ioe.Message.Contains("CodeInternal"))
        {
            return Conflict(new { ok = false, error = "CodeInternal already exists" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT /v1/properties/{Id} failed", id);
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }

    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> ChangePrice([FromRoute] Guid id, [FromBody] PropertyChangePriceRequestDto request, CancellationToken ct)
    {
        try
        {
            await _changePrice.HandleAsync(id, request, ct);
            return Ok(new { ok = true, id, newPrice = request.NewPrice, message = "Price updated" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { ok = false, error = "Property not found" });
        }
        catch (InvalidOperationException ioe) when (ioe.Message.Contains("Price"))
        {
            return BadRequest(new { ok = false, error = "Price must be positive" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PATCH /v1/properties/{Id}/price failed", id);
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }

    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> AddImage([FromRoute] Guid id, [FromBody] PropertyAddImageRequestDto request, CancellationToken ct)
    {
        try
        {
            var imageId = await _addImage.HandleAsync(id, request, ct);
            return Created($"/v1/properties/{id}/images/{imageId}", new { ok = true, id = imageId, message = "Image added" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { ok = false, error = "Property not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST /v1/properties/{Id}/images failed", id);
            return StatusCode(500, new { ok = false, error = "Unexpected error" });
        }
    }
}
