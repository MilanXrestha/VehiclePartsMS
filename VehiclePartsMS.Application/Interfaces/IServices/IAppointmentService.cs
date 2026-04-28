using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<AppointmentResponseDto?> GetByIdAsync(long id);
    Task<AppointmentResponseDto> CreateAsync(AppointmentCreateDto dto);
    Task UpdateStatusAsync(long id, AppointmentUpdateDto dto);
}
