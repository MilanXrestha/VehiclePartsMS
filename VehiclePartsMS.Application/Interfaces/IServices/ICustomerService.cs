using VehiclePartsMS.Application.DTOs;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
    Task<CustomerWithDetailsDto?> GetWithDetailsAsync(long id);
    Task<CustomerHistoryDto?> GetHistoryAsync(long id);
    Task<IEnumerable<CustomerResponseDto>> SearchAsync(string? name, string? phone, string? vehicleNumber, long? userId);
    Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto);
    Task UpdateAsync(long id, CustomerUpdateDto dto);
    Task AddVehicleAsync(long customerProfileId, VehicleCreateDto dto);
}
