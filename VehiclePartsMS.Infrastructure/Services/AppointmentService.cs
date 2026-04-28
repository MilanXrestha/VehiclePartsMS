using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class AppointmentService(IAppointmentRepository repo) : IAppointmentService
{
    public Task<IEnumerable<AppointmentResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(long customerProfileId)
        => repo.GetByCustomerAsync(customerProfileId);

    public Task<AppointmentResponseDto?> GetByIdAsync(long id) => repo.GetByIdAsync(id);

    public async Task<AppointmentResponseDto> CreateAsync(AppointmentCreateDto dto)
    {
        var appointment = new ServiceAppointment
        {
            CustomerProfileId = dto.CustomerProfileId,
            VehicleId         = dto.VehicleId,
            AppointmentDate   = dto.AppointmentDate,
            Status            = AppointmentStatus.Pending,
            Notes             = dto.Notes,
            CreatedAt         = DateTime.UtcNow
        };
        var created = await repo.AddAsync(appointment);
        return (await repo.GetByIdAsync(created.Id))!;
    }

    public async Task UpdateStatusAsync(long id, AppointmentUpdateDto dto)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Appointment {id} not found.");

        existing.Status = dto.Status;
        if (dto.Notes is not null)
            existing.Notes = dto.Notes;

        await repo.UpdateAsync(existing);
    }
}
