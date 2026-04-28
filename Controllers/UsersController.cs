using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService service) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        => Ok(await service.GetAllAsync());

    // GET: api/users/staff
    [HttpGet("staff")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetStaff()
        => Ok(await service.GetStaffAsync());

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(long id)
    {
        var user = await service.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> PostUser(UserCreateDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(long id, UserUpdateDto dto)
    {
        try   { await service.UpdateAsync(id, dto); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        try   { await service.DeleteAsync(id); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
