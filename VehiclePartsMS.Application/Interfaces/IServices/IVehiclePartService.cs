using VehiclePartsMS.Application.DTOs;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface IVehiclePartService
{
    Task<IEnumerable<VehiclePartResponseDto>> GetAllAsync();
    Task<IEnumerable<VehiclePartResponseDto>> GetByCategoryAsync(string category);
    Task<VehiclePartResponseDto?> GetByIdAsync(long id);
    Task<VehiclePartResponseDto> CreateAsync(VehiclePartCreateDto dto);
    Task UpdateAsync(long id, VehiclePartUpdateDto dto);
    Task DeleteAsync(long id);
}
