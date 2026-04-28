using VehiclePartsMS.Application.DTOs;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface IPartRequestService
{
    Task<IEnumerable<PartRequestResponseDto>> GetAllAsync();
    Task<IEnumerable<PartRequestResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<PartRequestResponseDto?> GetByIdAsync(long id);
    Task<PartRequestResponseDto> CreateAsync(PartRequestCreateDto dto);
    Task UpdateStatusAsync(long id, PartRequestUpdateDto dto);
}
