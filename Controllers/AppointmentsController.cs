using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    // GET: api/appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetAppointments()
        => Ok(await service.GetAllAsync());

    // GET: api/appointments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentResponseDto>> GetAppointment(long id)
    {
        var appointment = await service.GetByIdAsync(id);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    // GET: api/appointments/customer/{customerProfileId}
    [HttpGet("customer/{customerProfileId}")]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetByCustomer(long customerProfileId)
        => Ok(await service.GetByCustomerAsync(customerProfileId));

    // POST: api/appointments
    [HttpPost]
    public async Task<ActionResult<AppointmentResponseDto>> PostAppointment(AppointmentCreateDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAppointment), new { id = created.Id }, created);
    }

    // PATCH: api/appointments/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, AppointmentUpdateDto dto)
    {
        try   { await service.UpdateStatusAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
