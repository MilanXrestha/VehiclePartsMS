using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface IAppointmentRepository
{
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<AppointmentResponseDto?> GetByIdAsync(long id);
    Task<ServiceAppointment?> FindByIdAsync(long id);
    Task<ServiceAppointment> AddAsync(ServiceAppointment appointment);
    Task UpdateAsync(ServiceAppointment appointment);
    Task<bool> ExistsAsync(long id);
}
