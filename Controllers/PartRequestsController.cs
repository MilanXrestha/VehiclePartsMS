using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartRequestsController(IPartRequestService service) : ControllerBase
{
    // GET: api/partrequests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartRequestResponseDto>>> GetPartRequests()
        => Ok(await service.GetAllAsync());

    // GET: api/partrequests/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PartRequestResponseDto>> GetPartRequest(long id)
    {
        var request = await service.GetByIdAsync(id);
        return request is null ? NotFound() : Ok(request);
    }

    // GET: api/partrequests/customer/{customerProfileId}
    [HttpGet("customer/{customerProfileId}")]
    public async Task<ActionResult<IEnumerable<PartRequestResponseDto>>> GetByCustomer(long customerProfileId)
        => Ok(await service.GetByCustomerAsync(customerProfileId));

    // POST: api/partrequests
    [HttpPost]
    public async Task<ActionResult<PartRequestResponseDto>> PostPartRequest(PartRequestCreateDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPartRequest), new { id = created.Id }, created);
    }

    // PATCH: api/partrequests/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, PartRequestUpdateDto dto)
    {
        try   { await service.UpdateStatusAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
