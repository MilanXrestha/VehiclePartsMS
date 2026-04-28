using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class VehiclePartRepository(ApplicationDbContext context) : IVehiclePartRepository
{
    public async Task<IEnumerable<VehiclePartResponseDto>> GetAllAsync()
        => await context.VehicleParts
            .Include(p => p.Vendor)
            .Select(p => new VehiclePartResponseDto(p.Id, p.Name, p.Description, p.Category, p.Price, p.StockQuantity, p.VendorId, p.Vendor.Name))
            .ToListAsync();

    public async Task<IEnumerable<VehiclePartResponseDto>> GetByCategoryAsync(string category)
        => await context.VehicleParts
            .Include(p => p.Vendor)
            .Where(p => p.Category == category)
            .Select(p => new VehiclePartResponseDto(p.Id, p.Name, p.Description, p.Category, p.Price, p.StockQuantity, p.VendorId, p.Vendor.Name))
            .ToListAsync();

    public async Task<VehiclePartResponseDto?> GetByIdAsync(long id)
        => await context.VehicleParts
            .Include(p => p.Vendor)
            .Where(p => p.Id == id)
            .Select(p => new VehiclePartResponseDto(p.Id, p.Name, p.Description, p.Category, p.Price, p.StockQuantity, p.VendorId, p.Vendor.Name))
            .FirstOrDefaultAsync();

    public async Task<VehiclePart?> FindByIdAsync(long id)
        => await context.VehicleParts.FindAsync(id);

    public async Task<VehiclePart> AddAsync(VehiclePart part)
    {
        context.VehicleParts.Add(part);
        await context.SaveChangesAsync();
        return part;
    }

    public async Task UpdateAsync(VehiclePart part)
    {
        context.VehicleParts.Update(part);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(VehiclePart part)
    {
        context.VehicleParts.Remove(part);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.VehicleParts.AnyAsync(p => p.Id == id);
}
