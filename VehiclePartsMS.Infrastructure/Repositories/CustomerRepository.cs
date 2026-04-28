using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Select(cp => new CustomerResponseDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance))
            .ToListAsync();

    public async Task<CustomerProfile?> FindByIdAsync(long id)
        => await context.CustomerProfiles.Include(cp => cp.User).FirstOrDefaultAsync(cp => cp.Id == id);

    public async Task<CustomerWithDetailsDto?> GetWithDetailsAsync(long id)
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Include(cp => cp.Vehicles)
            .Where(cp => cp.Id == id)
            .Select(cp => new CustomerWithDetailsDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance,
                cp.Vehicles.Select(v => new VehicleResponseDto(v.Id, v.VehicleNumber, v.Make, v.Model, v.Year))))
            .FirstOrDefaultAsync();

    public async Task<CustomerHistoryDto?> GetHistoryAsync(long id)
    {
        var cp = await context.CustomerProfiles
            .Include(c => c.User)
            .Include(c => c.SalesInvoices)
            .Include(c => c.Appointments).ThenInclude(a => a.Vehicle)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cp is null) return null;

        return new CustomerHistoryDto(
            cp.Id, cp.User.FirstName, cp.User.LastName,
            cp.SalesInvoices.Select(si => new SalesInvoiceSummaryDto(
                si.Id, si.SaleDate, si.TotalAmount, si.DiscountPercent, si.PaymentStatus)),
            cp.Appointments.Select(a => new AppointmentResponseDto(
                a.Id, a.CustomerProfileId, $"{cp.User.FirstName} {cp.User.LastName}",
                a.VehicleId, a.Vehicle != null ? a.Vehicle.VehicleNumber : null,
                a.AppointmentDate, a.Status, a.Notes, a.CreatedAt)));
    }

    public async Task<IEnumerable<CustomerResponseDto>> SearchByNameAsync(string name)
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Where(cp => (cp.User.FirstName + " " + cp.User.LastName).Contains(name))
            .Select(cp => new CustomerResponseDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance))
            .ToListAsync();

    public async Task<IEnumerable<CustomerResponseDto>> SearchByPhoneAsync(string phone)
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Where(cp => cp.User.Phone != null && cp.User.Phone.Contains(phone))
            .Select(cp => new CustomerResponseDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance))
            .ToListAsync();

    public async Task<IEnumerable<CustomerResponseDto>> SearchByVehicleNumberAsync(string vehicleNumber)
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Include(cp => cp.Vehicles)
            .Where(cp => cp.Vehicles.Any(v => v.VehicleNumber.Contains(vehicleNumber)))
            .Select(cp => new CustomerResponseDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance))
            .ToListAsync();

    public async Task<IEnumerable<CustomerResponseDto>> SearchByIdAsync(long userId)
        => await context.CustomerProfiles
            .Include(cp => cp.User)
            .Where(cp => cp.UserId == userId)
            .Select(cp => new CustomerResponseDto(
                cp.Id, cp.UserId, cp.User.FirstName, cp.User.LastName, cp.User.Email,
                cp.User.Phone, cp.Address, cp.TotalSpent, cp.CreditBalance))
            .ToListAsync();

    public async Task<CustomerProfile> AddAsync(CustomerProfile customer)
    {
        context.CustomerProfiles.Add(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateAsync(CustomerProfile customer)
    {
        context.CustomerProfiles.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.CustomerProfiles.AnyAsync(cp => cp.Id == id);
}
