using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService service) : ControllerBase
{
    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers(
        [FromQuery] string? name,
        [FromQuery] string? phone,
        [FromQuery] string? vehicleNumber,
        [FromQuery] long?   userId)
        => Ok(await service.SearchAsync(name, phone, vehicleNumber, userId));

    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerWithDetailsDto>> GetCustomer(long id)
    {
        var customer = await service.GetWithDetailsAsync(id);
        return customer is null ? NotFound() : Ok(customer);
    }

    // GET: api/customers/{id}/history
    [HttpGet("{id}/history")]
    public async Task<ActionResult<CustomerHistoryDto>> GetCustomerHistory(long id)
    {
        var history = await service.GetHistoryAsync(id);
        return history is null ? NotFound() : Ok(history);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> PostCustomer(CustomerCreateDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCustomer), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    // PUT: api/customers/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(long id, CustomerUpdateDto dto)
    {
        try   { await service.UpdateAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    // POST: api/customers/{id}/vehicles
    [HttpPost("{id}/vehicles")]
    public async Task<IActionResult> AddVehicle(long id, VehicleCreateDto dto)
    {
        try   { await service.AddVehicleAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
