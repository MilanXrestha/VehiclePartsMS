using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    private static AppointmentResponseDto MapToDto(ServiceAppointment a) =>
        new(a.Id, a.CustomerProfileId,
            a.CustomerProfile.User.FirstName + " " + a.CustomerProfile.User.LastName,
            a.VehicleId, a.Vehicle != null ? a.Vehicle.VehicleNumber : null,
            a.AppointmentDate, a.Status, a.Notes, a.CreatedAt);

    private IQueryable<ServiceAppointment> WithIncludes()
        => context.ServiceAppointments
            .Include(a => a.CustomerProfile).ThenInclude(cp => cp.User)
            .Include(a => a.Vehicle);

    public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
        => (await WithIncludes().ToListAsync()).Select(MapToDto);

    public async Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(long customerProfileId)
        => (await WithIncludes().Where(a => a.CustomerProfileId == customerProfileId).ToListAsync()).Select(MapToDto);

    public async Task<AppointmentResponseDto?> GetByIdAsync(long id)
    {
        var a = await WithIncludes().FirstOrDefaultAsync(a => a.Id == id);
        return a is null ? null : MapToDto(a);
    }

    public async Task<ServiceAppointment?> FindByIdAsync(long id)
        => await context.ServiceAppointments.FindAsync(id);

    public async Task<ServiceAppointment> AddAsync(ServiceAppointment appointment)
    {
        context.ServiceAppointments.Add(appointment);
        await context.SaveChangesAsync();
        return appointment;
    }

    public async Task UpdateAsync(ServiceAppointment appointment)
    {
        context.ServiceAppointments.Update(appointment);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.ServiceAppointments.AnyAsync(a => a.Id == id);
}
