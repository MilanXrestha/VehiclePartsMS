using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface IPartRequestRepository
{
    Task<IEnumerable<PartRequestResponseDto>> GetAllAsync();
    Task<IEnumerable<PartRequestResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<PartRequestResponseDto?> GetByIdAsync(long id);
    Task<PartRequest?> FindByIdAsync(long id);
    Task<PartRequest> AddAsync(PartRequest request);
    Task UpdateAsync(PartRequest request);
    Task<bool> ExistsAsync(long id);
}
