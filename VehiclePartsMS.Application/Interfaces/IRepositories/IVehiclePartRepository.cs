using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface IVehiclePartRepository
{
    Task<IEnumerable<VehiclePartResponseDto>> GetAllAsync();
    Task<IEnumerable<VehiclePartResponseDto>> GetByCategoryAsync(string category);
    Task<VehiclePartResponseDto?> GetByIdAsync(long id);
    Task<VehiclePart?> FindByIdAsync(long id);
    Task<VehiclePart> AddAsync(VehiclePart part);
    Task UpdateAsync(VehiclePart part);
    Task DeleteAsync(VehiclePart part);
    Task<bool> ExistsAsync(long id);
}
