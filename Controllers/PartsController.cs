using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartsController(IVehiclePartService service) : ControllerBase
{
    // GET: api/parts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehiclePartResponseDto>>> GetParts([FromQuery] string? category)
    {
        if (!string.IsNullOrWhiteSpace(category))
            return Ok(await service.GetByCategoryAsync(category));
        return Ok(await service.GetAllAsync());
    }

    // GET: api/parts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VehiclePartResponseDto>> GetPart(long id)
    {
        var part = await service.GetByIdAsync(id);
        return part is null ? NotFound() : Ok(part);
    }

    // POST: api/parts
    [HttpPost]
    public async Task<ActionResult<VehiclePartResponseDto>> PostPart(VehiclePartCreateDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPart), new { id = created.Id }, created);
    }

    // PUT: api/parts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPart(long id, VehiclePartUpdateDto dto)
    {
        try   { await service.UpdateAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    // DELETE: api/parts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(long id)
    {
        try   { await service.DeleteAsync(id); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
