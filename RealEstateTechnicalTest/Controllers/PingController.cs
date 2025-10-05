using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;

namespace Api.Controllers;

[ApiController]
[Route("ping")]
public sealed class PingController : ControllerBase
{
    private readonly RealEstateDbContext _db;
    public PingController(RealEstateDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var canConnect = await _db.Database.CanConnectAsync(ct);
        return Ok(new { ok = true, db = canConnect, at = DateTime.UtcNow });
    }
}
