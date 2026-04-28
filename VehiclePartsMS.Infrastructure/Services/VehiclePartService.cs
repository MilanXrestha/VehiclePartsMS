using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class VehiclePartService(IVehiclePartRepository repo) : IVehiclePartService
{
    public Task<IEnumerable<VehiclePartResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<IEnumerable<VehiclePartResponseDto>> GetByCategoryAsync(string category)
        => repo.GetByCategoryAsync(category);

    public Task<VehiclePartResponseDto?> GetByIdAsync(long id) => repo.GetByIdAsync(id);

    public async Task<VehiclePartResponseDto> CreateAsync(VehiclePartCreateDto dto)
    {
        var part = new VehiclePart
        {
            Name          = dto.Name,
            Description   = dto.Description,
            Category      = dto.Category,
            Price         = dto.Price,
            StockQuantity = dto.StockQuantity,
            VendorId      = dto.VendorId
        };
        var created = await repo.AddAsync(part);
        return (await repo.GetByIdAsync(created.Id))!;
    }

    public async Task UpdateAsync(long id, VehiclePartUpdateDto dto)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Part {id} not found.");

        existing.Name          = dto.Name;
        existing.Description   = dto.Description;
        existing.Category      = dto.Category;
        existing.Price         = dto.Price;
        existing.StockQuantity = dto.StockQuantity;
        existing.VendorId      = dto.VendorId;

        await repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(long id)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Part {id} not found.");
        await repo.DeleteAsync(existing);
    }
}
